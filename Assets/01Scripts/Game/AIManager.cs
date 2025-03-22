using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager instance
    {
        get; private set;
    }

    public bool SinglePlay;

    [SerializeField] CardManager cardManager;
    [SerializeField] EntityManager entityManager;
    [SerializeField] MapManager mapManager;
    [SerializeField] GameManager gameManager;
    [SerializeField] TurnManager turnManager;

    CardMove cardMove = new CardMove();

    public List<Hand> hands;

    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        TurnManager.OnTurnStarted -= TurnEndSetup;
    }
    void Init()
    {
        if (SinglePlay)
        {
            AI_Setting_Outpost();
            gameManager.clickBlock = false;

            TurnManager.OnTurnStarted += TurnEndSetup;
        }
    }

    public void Entity_Active(List<Entity> allEntities)
    {
        List<Entity> entities = new List<Entity>();
        foreach (Entity entity in allEntities)
        {
            if (entity.belong == EntityBelong.AI)
            {
                entities.Add(entity);
            }
        }

        int randomIndex = Random.Range(0, entities.Count);
        
        for (int i = 0; i < entities.Count; i++)
        {
            Debug.Log($"엔티티 개수 : {entities.Count}, 인덱스 : {randomIndex}, 문제 확인용 인덱스 문제가 있음");
            Random_Active(entities[randomIndex], true);
            randomIndex = Random.Range(0, entities.Count);
        }
    }


    Coordinate RandomCoord(int mapSize)
    {
        int x, y;

        x = Random.Range(0, mapSize);
        y = Random.Range(mapSize / 2, mapSize);

        return new Coordinate(x, y);
    }
    public void AI_Setting_Outpost()
    {
        int mapSize = MapManager.instance.mapSize;
        
        Coordinate AI_OutpostPos1 = RandomCoord(mapSize);
        Coordinate AI_OutpostPos2;
        while (true)
        {
            AI_OutpostPos2 = RandomCoord(mapSize);
            if (AI_OutpostPos1.vector3Pos != AI_OutpostPos2.vector3Pos)
            {
                break;
            }
        }

        MapManager.instance.SetOutpost(AI_OutpostPos1, false);
        MapManager.instance.SetOutpost(AI_OutpostPos2, false);
    }

    [ContextMenu("턴액션")]
    public void TurnEndSetup(bool isTurn)
    {
        if (isTurn == false)
        {
            StartCoroutine(TurnAction());
        }
    }

    // 컴퓨터 카드 제거 그래픽이 원활치 못함

    IEnumerator TurnAction()
    {
        yield return new WaitForSeconds(2f);

        int summonCount = 0;
        int actionCount = 0;

        Hand[] hands = new Hand[cardManager.opponentCards.Count];
        cardManager.opponentCards.CopyTo(hands);
        List<Coordinate> canSummonPos = new List<Coordinate>();
        List<Entity> attackEntity = new List<Entity>();
        List<Entity> tribute = new List<Entity>();

        foreach (Entity entity in entityManager.opponentEntities)
        {
            if (entity.card.cost == 0)
            {
                tribute.Add(entity);
            }
        }

        foreach (Coordinate summonCoord in mapManager.enermySummonPos)
        {
            if (mapManager.GetTile(summonCoord).onEntity == null)
            {
                canSummonPos.Add(summonCoord);
            }
        }

        foreach (Hand hand in hands)
        {
            if (entityManager.canUseCard(false, hand.card))
            {
                if (hand.card.cardType.card_category == CardCategory.Monster)
                {
                    if (hand.card.cost <= tribute.Count)
                    {
                        cardManager.TryPutCard(false, hand.card.id, canSummonPos[Random.Range(0, canSummonPos.Count)]);
                        summonCount++;
                    }

                    yield return new WaitForSeconds(1.5f);

                    if (summonCount >= 2)
                    {
                        break;
                    }
                }
            }
        }

        List<Entity> attackableEntities = new List<Entity>();

        foreach (Entity entity in entityManager.opponentEntities)
        {
            if (entity.attackable)
            {
                attackableEntities.Add(entity);
            }
        }

        // 적 제일 가까운놈 있으면 걔는 공격하게 해야함
        foreach (Entity searchEntity in attackableEntities)
        {
            if (AttackableTarget(searchEntity))
            {
                attackEntity.Add(searchEntity);
            }
        }

        for (int i = 0; i < attackEntity.Count; i++)
        {
            int randomIndex = Random.Range(0, attackEntity.Count);

            if (attackEntity[randomIndex].attackable)
            {
                Random_Active(attackEntity[randomIndex], false);
                actionCount++;
                yield return new WaitForSeconds(2f);
            }

            if (actionCount >= entityManager.MaxMoveCount)
            {
                break;
            }
        }

        if (actionCount < entityManager.MaxMoveCount)
        {
            for (int i = 0; i < attackableEntities.Count; i++)
            {
                int randomIndex = Random.Range(0, attackableEntities.Count);

                if (attackableEntities[randomIndex].attackable)
                {
                    Random_Active(attackableEntities[randomIndex], false);
                    actionCount++;
                    yield return new WaitForSeconds(2f);
                }

                if (actionCount >= entityManager.MaxMoveCount)
                {
                    break;
                }
            }
        }
        turnManager.StartTurn();
    }

    public void SelectTribute(List<Entity> targetEntities, Card card)
    {
        List<Entity> tribute = new List<Entity>();

        foreach (Entity entity in targetEntities)
        {
            if (entity.card.cost == 0)
            {
                tribute.Add(entity);
            }

            if (card.cost <= tribute.Count)
            {
                break;
            }
        }

        foreach (Entity entity in tribute)
        {
            entityManager.SelectMonster(false, entity.id);
        }
    }


    // 아마 이동 불가능 할때도 그냥 한번 행동한것으로 해서 다음 행동 안할꺼임, 근데 그냥 그것도 나쁘진 않을꺼 같아서 수정안함
    // 만약 문제가 있다면 여기서부터 손보면 될것임
    void Random_Active(Entity entity, bool isNeutrality)
    {
        List<Coordinate> canMovePos = cardMove.Can_Attack_Position(entity);
        int randomIndex = Random.Range(0, canMovePos.Count);
        List<Entity> targetEntities = new List<Entity>();
        List<Outpost> targetOutposts = new List<Outpost>();

        if (canMovePos.Count <= 0 && entity.belong != EntityBelong.AI)
            return;

        foreach (Coordinate pos in canMovePos)
        {
            if (mapManager.GetTile(pos).onEntity != null || mapManager.GetTile(pos).outpost.life > 0)
            {
                if (isNeutrality)
                {
                    if (mapManager.GetTile(pos).onEntity != null && mapManager.GetTile(pos).onEntity.belong != entity.belong)
                    {
                        targetEntities.Add(mapManager.GetTile(pos).onEntity);
                    }
                    else if (mapManager.GetTile(pos).outpost.life > 0)
                    {
                        targetOutposts.Add(mapManager.GetTile(pos).outpost);
                    }
                }
                else
                {
                    if (mapManager.GetTile(pos).onEntity != null && mapManager.GetTile(pos).onEntity.belong != entity.belong)
                    {
                        targetEntities.Add(mapManager.GetTile(pos).onEntity);
                    }
                    else if (mapManager.GetTile(pos).outpost.life > 0 && mapManager.GetTile(pos).outpost.belong != entity.belong)
                    {
                        targetOutposts.Add(mapManager.GetTile(pos).outpost);
                    }
                }
            }
        }

        if (GameManager.instance.MultiMode)
        {
            bool randomBool = (Random.value > 0.5f);
            if (targetOutposts.Count > 0 && targetEntities.Count > 0)
            {
                if (randomBool)
                {
                    Outpost target = targetOutposts[Random.Range(0, targetEntities.Count)];
                    gameManager.localGamePlayerScript.CmdOutpostAttack(entity.id, target.coordinate.vector3Pos, true);
                }
                else
                {
                    Entity target = targetEntities[Random.Range(0, targetEntities.Count)];
                    gameManager.localGamePlayerScript.CmdAttack(entity.id, target.id, true);
                }
            }

            else if (targetOutposts.Count > 0)
            {
                Outpost target = targetOutposts[Random.Range(0, targetEntities.Count)];
                gameManager.localGamePlayerScript.CmdOutpostAttack(entity.id, target.coordinate.vector3Pos, true);
            }

            else if (targetEntities.Count > 0)
            {
                Entity target = targetEntities[Random.Range(0, targetEntities.Count)];
                gameManager.localGamePlayerScript.CmdAttack(entity.id, target.id, true);
            }
            else
            {
                gameManager.localGamePlayerScript.CmdCardMove(entity.id, false, canMovePos[randomIndex].vector3Pos, true);
            }
        }

        else if (SinglePlay)
        {
            if (targetOutposts.Count > 0 && targetEntities.Count > 0)
            {
                bool randomBool = (Random.value > 0.5f);
                if (randomBool)
                {
                    Outpost target = targetOutposts[Random.Range(0, targetOutposts.Count)];
                    entityManager.OutpostAttack(entity.id, target.coordinate, true);
                }
                else
                {
                    Entity target = targetEntities[Random.Range(0, targetEntities.Count)];
                    entityManager.Attack(entity.id, target.id, true);
                }
            }

            else if (targetOutposts.Count > 0)
            {
                Outpost target = targetOutposts[Random.Range(0, targetOutposts.Count)];
                entityManager.OutpostAttack(entity.id, target.coordinate, true);
            }

            else if (targetEntities.Count > 0)
            {
                Entity target = targetEntities[Random.Range(0, targetEntities.Count)];
                entityManager.Attack(entity.id, target.id, true);
            }
            else
            {
                Coordinate coordinate = MinDistanceCoord(canMovePos);
                entityManager.CardMove(entity.id, coordinate, false);
            }

        }
    }

    bool AttackableTarget(Entity entity)
    {
        List<Coordinate> canMovePos = cardMove.Can_Attack_Position(entity);
        List<Entity> targetEntities = new List<Entity>();
        List<Outpost> targetOutposts = new List<Outpost>();

        foreach (Coordinate pos in canMovePos)
        {
            if (mapManager.GetTile(pos).onEntity != null || mapManager.GetTile(pos).outpost.life > 0)
            {
                if (mapManager.GetTile(pos).onEntity != null && mapManager.GetTile(pos).onEntity.belong == EntityBelong.Player)
                {
                    targetEntities.Add(mapManager.GetTile(pos).onEntity);
                }
                else if (mapManager.GetTile(pos).outpost.life > 0 && mapManager.GetTile(pos).outpost.belong == EntityBelong.Player)
                {
                    targetOutposts.Add(mapManager.GetTile(pos).outpost);
                }
            }
        }

        if (targetEntities.Count > 0 || targetOutposts.Count > 0)
        {
            return true;
        }
        return false;
    }

    Coordinate MinDistanceCoord(List<Coordinate> canMovePos)
    {
        List<Coordinate> enermiesPos = new List<Coordinate>();
        int rand = Random.Range(0, canMovePos.Count);
        Coordinate minCoord = canMovePos[rand];

        int minDistance = 100;

        foreach (Entity entity in entityManager.playerEntities)
        {
            enermiesPos.Add(entity.coordinate);
        }

        foreach (Coordinate movePos in canMovePos)
        {
            foreach (Coordinate enermyCoord in enermiesPos)
            {
                int distance = System.Math.Abs(movePos.x - enermyCoord.x) + System.Math.Abs(movePos.y - enermyCoord.y);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    minCoord = movePos;
                }
            }

            foreach (Outpost outpost in mapManager.player_OutpostList)
            {
                int distance = System.Math.Abs(movePos.x - outpost.coordinate.x) + System.Math.Abs(movePos.y - outpost.coordinate.y);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    minCoord = movePos;
                }
            }
        }

        return minCoord;
    }

    void Use_Hand()
    {
    
    }
}
