using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class EntityManager : MonoBehaviour
{
    public static EntityManager instance
    {
        get; set;
    }

    private void Awake()
    {
        instance = this;
    }

    public GraveManager graveManager;
    public AIManager aiManager;
    public EffectManager effectManager;

    [SerializeField] CameraEffect cameraEffect;
    [SerializeField] GameObject entityPrefab;
    [SerializeField] Transform CardCreatePos;
    [SerializeField] Transform playerGraveyard;
    [SerializeField] Transform oppenentGraveyard;
    [SerializeField] GameObject TargetPicker;

    [SerializeField][Header("플레이어 엔티티 부모")] Transform playerEntityParent;
    [SerializeField][Header("적군 엔티티 부모")] Transform opponentEntityParent;
    [SerializeField][Header("중립 엔티티 부모")] Transform neutralityEntityParent;

    [SerializeField] private int _maxMoveCount;
    [SerializeField] private int _maxSummonCount;
    public int MaxMoveCount
    {
        get
        {
            return _maxMoveCount;
        }
        private set
        {
            _maxMoveCount = value;
        }
    }
    public int MaxSummonCount
    {
        get
        {
            return _maxSummonCount;
        }
        private set
        {
            _maxSummonCount = value;
        }
    }

    public TMP_Text canMoveCountTMP;
    public TMP_Text canSummonCountTMP;

    public List<Entity> playerEntities;
    public List<Entity> opponentEntities;
    public List<Entity> aiEntities;

    public Tile selectTile;
    Coordinate entityCoordinateData;

    // 카드 로그
    public PlayLogControl gameLog;

    public bool SelectMonsterMode = false;

    CardMove cardMove = new CardMove();

    public bool clickBlock;

    Entity selectEntity = null;
    public bool selectState => selectEntity != null;

    public Entity enermyEntityPick;

    public List<Entity> tributeEntities = new List<Entity>();

    [HideInInspector]
    public int summonCount, canMoveCount, entityIDCount, cardCost, effect_Count;

    // cardCost, card_id , effectCard 는 없앨수 있을꺼 같음
    // 왜 따로??
    string card_id; // 제물 및 타 효과시 임시 카드 저장 정보
    public Card effectCard; // 카드 효과 발동시 정보를 임시 저장하는 변수

    public Dictionary<Tribe, int> tribeSummonCount;
    public Dictionary<Tribe, int> tribeTributeCount;

    [Header("이펙트 모음")]
    [SerializeField] GameObject death_VFX;
    [SerializeField] GameObject explosion_VFX;
    [SerializeField] GameObject arrow_VFX;
    public GameObject portal_VFX;

    [SerializeField][Header("원거리 공격 이펙트")] GameObject bullet_Obj;

    private void Start()
    {
        Init();
    }
    private void Update()
    {
        //if (enermyEntityPick != null || (selectTile != null))
        //{
        //    ShowTargetPicker(true);
        //}
        //else
        //{
        //    ShowTargetPicker(false);
        //}
    }
    private void OnDestroy()
    {
        TurnManager.OnTurnStarted -= OnTurnStarted;
    }

    void Init()
    {
        canMoveCount = MaxMoveCount;
        summonCount = MaxSummonCount;
        canMoveCountTMP.text = "0";
        canSummonCountTMP.text = "0";
        TurnManager.OnTurnStarted += OnTurnStarted;
        opponentEntities = new List<Entity>();
        aiEntities = new List<Entity>();
        tribeSummonCount = new Dictionary<Tribe, int>();
        tribeTributeCount = new Dictionary<Tribe, int>();
        entityIDCount = 0;
        cardCost = 0;

        effect_Count = 0;

        clickBlock = false;
        //moveEffect = false;
    }

    void CountTMP_Update()
    {
        canMoveCountTMP.text = canMoveCount.ToString();
        canSummonCountTMP.text = summonCount.ToString();
    }

    public void MouseEventInit()
    {
        //ChangeColor_CanMovePos(false, selectEntity);
        selectEntity = null;
        enermyEntityPick = null;
    }

    public List<Entity> All_Entities
    {
        get
        {
            List<Entity> allEntities = new List<Entity>(playerEntities);
            allEntities.AddRange(opponentEntities);
            allEntities.AddRange(aiEntities);

            return allEntities;
        }
    }

    #region Entity 마우스 관련
    public void EntityMouseDown(Entity entity)
    {
        if (clickBlock)
            return;

        selectEntity = entity;
        if (SelectMonsterMode) { return; }

        if (entity.belong == EntityBelong.Player && entity.attackable && canMoveCount > 0 && TurnManager.instance.myTurn)
        {
            ChangeColor_CanMovePos(true, entity);
        }
        else
        {
            ChangeColor_CanMovePos(true, entity, false);
        }

    }

    public void EntityMouseUP(Entity entity)
    {
        effectManager.Select_Effect_Target(entity);

        if (entity.isMine && SelectMonsterMode)
        {
            if (entity.canTribute && entity.isDie == false)
            {
                DarkTonic.MasterAudio.MasterAudio.PlaySound("ButtonClick");
                if (GameManager.instance.MultiMode)
                {
                    GameManager.instance.localGamePlayerScript.CmdSelectTribute(entity.id, NetworkRpcFunc.instance.isServer);
                }
                else
                {
                    SelectMonster(entity.isMine, entity.id);
                }
                return;
            }
        }
        else
        {
            ChangeColor_CanMovePos(false, entity);
        }

        if (clickBlock) { return; }

        Entity_Active();
        selectEntity = null;
        enermyEntityPick = null;
    }

    public void EntityMouseOver(Entity entity)
    {
        if (selectEntity == null)
            return;
        if (clickBlock)
            return;
        
        if (entity.isMine == false)
        {
            enermyEntityPick = entity;
        }
    }

    public void EntityMouseExit(Entity entity)
    {
        if (entity.isMine == false)
        {
            enermyEntityPick = null;
        }
    }

    public void EntityMouseDrag()
    {
        //if (!CanMouseInput || selectEntity == null)
        //    return;
        //if (SelectMonsterMode)
        //    return;
        //if (clickBlock)
        //    return;
    }
    #endregion

    void OnTurnStarted(bool myTurn)
    {
        if (selectEntity != null)
            ChangeColor_CanMovePos(false, selectEntity);

        selectEntity = null;
        AttackableReset(myTurn);
        canMoveCount = MaxMoveCount;
        summonCount = MaxSummonCount;
        SelectMonsterMode = false;
        clickBlock = false;

        effectManager.effect_Activated = false;

        tributeEntities.Clear();
        tribeSummonCount.Clear();

        cardCost = 0;

        if (TurnManager.instance.firstTurn)
        {
            summonCount = MaxSummonCount - 1;
        }

        if (GameManager.instance.MultiMode)
        {
            if (NetworkRpcFunc.instance.isServer)
            {
                aiManager.Entity_Active(All_Entities);
            }
        }
        else
        {
            aiManager.Entity_Active(All_Entities);
        }
        
        effect_Count = 0;
        CountTMP_Update();
        UpdateEntitiesState();
    }

    // 카드 소환시 타일 좌표로 정렬시켜주는 함수
    void EntityAlignment(Entity targetEntity, Vector3 spawnPos)
    {
        targetEntity.transformPos = spawnPos;
        targetEntity.GetComponent<Order>().SetOriginOrder(10);
    }

    void ChangeColor_CanMovePos(bool changeColor, Entity entity, bool CanMove = true)
    {
        if (entity == null)
            return;

        foreach (Coordinate pos in cardMove.Can_Attack_Position(entity))
        {
            int x = (int)pos.x;
            int y = (int)pos.y;

            if (CanMove)
            {
                MapManager.instance.mapData[x, y].ColorChange_Rock(changeColor, Color.yellow);
            }
            else
            {
                MapManager.instance.mapData[x, y].ColorChange_Rock(changeColor, Color.black);
            }
        }

        foreach (Coordinate pos in cardMove.FindCanMovePositionList(entity))
        {
            int x = (int)pos.x;
            int y = (int)pos.y;

            if (CanMove)
            {
                MapManager.instance.mapData[x, y].ColorChange_Rock(changeColor, Color.green);
            }
            else
            {
                MapManager.instance.mapData[x, y].ColorChange_Rock(changeColor, Color.red);
            }
        }
    }

    void Entity_Active()
    {
        if (selectEntity == null || selectTile == null) { return; }
        if (canMoveCount <= 0) { return; }
        if (selectEntity.attackable == false) { return; }
        if(TurnManager.instance.myTurn == false) { return; }

        bool MultiMode = GameManager.instance.MultiMode;

        int entityID = selectEntity.id;
        Coordinate targetCoord = selectTile.coordinate;

        if (enermyEntityPick)
        {
            if (cardMove.Can_Attack_Position(selectEntity).Exists(x => x.vector3Pos == enermyEntityPick.coordinate.vector3Pos))
            {
                if (MultiMode)
                {
                    GameManager.instance.localGamePlayerScript.CmdAttack(entityID, enermyEntityPick.id, NetworkRpcFunc.instance.isServer);
                }
                else
                {
                    Attack(entityID, enermyEntityPick.id, selectEntity.isMine);
                }
            }
        }

        else if (selectTile.tileState == TileState.enermyOutpost)
        {
            if (cardMove.Can_Attack_Position(selectEntity).Exists(x => x.vector3Pos == selectTile.coordinate.vector3Pos))
            {
                if (MultiMode)
                {
                    GameManager.instance.localGamePlayerScript.CmdOutpostAttack(entityID, targetCoord.vector3Pos, NetworkRpcFunc.instance.isServer);
                }
                else
                {
                    OutpostAttack(entityID, targetCoord, selectEntity.isMine);
                }
            }
        }
        else
        {
            if (MultiMode)
            {
                GameManager.instance.localGamePlayerScript.CmdCardMove(entityID, selectEntity.isMine, targetCoord.vector3Pos, NetworkRpcFunc.instance.isServer);
                
            }
            else
            {
                CardMove(entityID, targetCoord, selectEntity.isMine);
            }
        }
    }

    public void CardMove(int entityID, Coordinate moveCoord, bool server)
    {
        bool isMine = GameManager.instance.IsMine(server);

        Entity targetEntity = All_Entities.Find(x => x.id == entityID);

        if (cardMove.Move(targetEntity, MapManager.instance.coordinateTile(moveCoord)))
        {
            if (targetEntity.belong != EntityBelong.AI)
            {
                canMoveCount--;
            }
        }
        CountTMP_Update();
    }

    #region 소환 관련
    void SummonBase(bool isMine, Card card, Tile summonTile)
    {
        Vector3 spawnPos = CardCreatePos.position;
        Transform entityParent = isMine ? playerEntityParent : opponentEntityParent;
        string sumnmonPlayer = isMine ? "Player : " : "Opponent : ";

        if (isMine == false)
        {
            spawnPos.y = -spawnPos.y;
        }

        GameObject entityObject = Instantiate(entityPrefab, spawnPos, Utils.QI, entityParent);
        entityObject.name = sumnmonPlayer + card.name + entityIDCount;

        Entity entity = entityObject.GetComponent<Entity>();
        entity.Setup(card, isMine);

        if (isMine)
        {
            playerEntities.Add(entity);
            int index = playerEntities.IndexOf(entity);
            playerEntities[index] = entity;
            entity.id = entityIDCount;
            entity.coordinate = summonTile.coordinate;
            if (SelectMonsterMode)
            {
                entity.coordinate = entityCoordinateData;
                Tile targetTile = MapManager.instance.coordinateTile(entityCoordinateData);
                targetTile.onEntity = entity;
                targetTile.tileState = TileState.onPlayerMonster;
            }
            summonTile.onEntity = entity;
            summonTile.tileState = TileState.onPlayerMonster;
        }
        else
        {
            opponentEntities.Add(entity);
            entity.id = entityIDCount;
            entity.coordinate = summonTile.coordinate;

            //summonTile.SetMonster(entity);
            summonTile.onEntity = entity;
            summonTile.tileState = TileState.onEnermyMonster;
            
        }

        if (tribeSummonCount.ContainsKey(card.cardType.tribe))
        {
            if (tribeSummonCount[card.cardType.tribe] > 0)
            {
                tribeSummonCount[card.cardType.tribe] = tribeSummonCount[card.cardType.tribe] - 1;
            }
            else
            {
                summonCount--;
            }
        }
        else
        {
            summonCount--;
        }

        entityIDCount++;
        effectManager.Add_Activated_Effect_To_Entity(entity);

        if (card.ability.effect_Time != EffectTime.Battle)
        {
            effectManager.EffectTrigger(isMine, card.id);
        }

        DarkTonic.MasterAudio.MasterAudio.PlaySound("Summon");
        CardManager.instance.Can_Use_Effect();
        EntityAlignment(entity, summonTile.transformPos);
        EnlargeCardManager.instance.Setup(card, true);
        CountTMP_Update();
        //UpdateEntityState();
    }

    // 카드 
    public bool canUseCard(bool isMine, Card card)
    {
        List<Entity> targetEntities = isMine ? playerEntities : opponentEntities;
        int tributeCount = targetEntities.Count;

        if (TurnManager.instance.myTurn != isMine)
        {
            return false;
        }

        if (card.cardType.card_category == CardCategory.Monster)
        {
            if (summonCount > 0)
            {
                foreach (Entity fieldCard in targetEntities)
                {
                    if (fieldCard.card.ability.effect_Time == EffectTime.Tribute)
                    {
                        if (fieldCard.card.ability.targetID == card.id)
                        {
                            return true;
                        }

                        foreach (Effect effect in fieldCard.card.ability.effects)
                        {
                            if (effect.effectClass == EffectClass.tribute)
                            {
                                if (card.cardType.tribe == fieldCard.card.cardType.tribe || fieldCard.card.cardType.tribe == Tribe.Common)
                                {
                                    tributeCount = tributeCount + effect.value;
                                }
                            }
                        
                        }
                    }
                }

                if (tribeTributeCount.ContainsKey(card.cardType.tribe))
                {
                    if (card.cost > 0 && tributeCount + tribeTributeCount[card.cardType.tribe] < card.cost)
                    {
                        return false;
                    }
                }
                else
                {
                    if (card.cost > 0 && tributeCount < card.cost)
                    {
                        return false;
                    }
                }
                
                return true;
            }

            if (tribeSummonCount.ContainsKey(card.cardType.tribe))
            {
                if (tribeSummonCount[card.cardType.tribe] > 0)
                {
                    if (card.cost > 0 && playerEntities.Count < card.cost)
                    {
                        return false;
                    }
                    return true;
                }
            }
        }
        else if (card.cardType.card_category == CardCategory.Magic)
        {
            if (card.cost > 0 && playerEntities.Count < card.cost)
            {
                return false;
            }
            return true;
        }

        // 조건 다 안걸렸다는 뜻
        return false;
    }

    bool canSummon(bool isMine, Card card, Tile tile)
    {
        if (tile == null || tile.tileState != TileState.empty)
            return false;

        if (tile.canSpawn == CanSpawn.nothing)
            return false;

        if (tile.canSpawn == CanSpawn.playerCanSpawn && isMine == false)
            return false;

        if (tile.canSpawn == CanSpawn.opponentCanSpawn && isMine)
            return false;

        if (tribeSummonCount.ContainsKey(card.cardType.tribe))
        {
            if (tribeSummonCount[card.cardType.tribe] > 0)
                return true;
        }

        if (summonCount <= 0)
            return false;

        return true;
    }


    public void Summon(bool isMine, string card_id, Coordinate coordinate)
    {
        Card card = DataManager.instance.CardData(card_id);
        int cost = card.cost;
        Tile targetTile = MapManager.instance.coordinateTile(coordinate);
        entityCoordinateData = coordinate;

        if (canSummon(isMine, card, targetTile) == false)
            return;

        this.card_id = card_id;

        if (tribeTributeCount.ContainsKey(card.cardType.tribe))
        {
            cost = cost - tribeTributeCount[card.cardType.tribe];
        }

        cardCost = cost;

        if (cost <= 0)
        {
            SummonBase(isMine, card, targetTile);
            CardManager.instance.RemoveTargetCards(isMine);
            return;
        }
        else
        {
            if (isMine)
            {
                if (canUseCard(isMine, card))
                {
                    Select_Monster(true);
                }
            }
            else
            {
                if (AIManager.instance.SinglePlay)
                {
                    if (canUseCard(isMine, card))
                    {
                        AIManager.instance.SelectTribute(opponentEntities, card);
                    }
                }
            }
        }
    }


    public void Select_Monster(bool isSummon)
    {
        GameManager.instance.Notification(LocalizationManager.instance.GetIngameText("PickMonster"));
        CardManager.instance.TributeSummonSet(false); // 제물소환시 소환할 카드 잠깐 안보이게함
        if (isSummon)
        {
            foreach (Entity fieldEntity in playerEntities)
            {
                if (fieldEntity.card.ability.targetID == card_id)
                {
                    fieldEntity.ClickMark(true);
                }
            }

            SelectMonsterMode = true;
        }
        else
        {
            effectManager.effect_Activated = true;
        }
    }

    void TributeSummon(bool isMine, string card_id)
    {
        Card card = DataManager.instance.CardData(card_id);

        CardManager.instance.RemoveTargetCards(isMine);


        foreach (Entity entity in tributeEntities)
        {
            entity.isDie = true;
            EntityState(entity);
        }

        Tile targetTile = MapManager.instance.coordinateTile(entityCoordinateData);

        SummonBase(isMine, card, targetTile);

        foreach (Entity entity in playerEntities)
        {
            entity.ClickMark(false);
        }

        tributeEntities.Clear();
        tribeTributeCount.Clear();

        cardCost = 0;
        SelectMonsterMode = false;
    }

    void TributeMagic(bool isMine, string card_id)
    {
        Card card = DataManager.instance.CardData(card_id);

        if (isMine == false && GameManager.instance.MultiMode)
        {
            CardManager.instance.Remove_OhterPlayer_HandCard();
        }

        CardManager.instance.RemoveTargetCards(isMine);

        foreach (var entity in tributeEntities)
        {
            entity.isDie = true;
        }

        if (GameManager.instance.MultiMode)
        {
            GameManager.instance.localGamePlayerScript.CmdEffectSolve(card.id, NetworkRpcFunc.instance.isServer);
        }
        else
        {
            effectManager.EffectSolve(card_id, isMine);
        }

        //UpdateEntityState();

        tributeEntities.Clear();
        cardCost = 0;
        SelectMonsterMode = false;
    }

    public void SelectMonster(bool server, int entityID)
    {
        bool isMine = GameManager.instance.IsMine(server);
        Entity entity = All_Entities.Find(x => x.id == entityID);

        entity.CheckMark(true);
        entity.ClickMark(false);
        entity.canTribute = false;

        tributeEntities.Add(entity);

        int tribute_Count = tributeEntities.Count;

        if (tribeTributeCount.ContainsKey(entity.card.cardType.tribe))
        {
            tribute_Count = tribute_Count + tribeTributeCount[entity.card.cardType.tribe];
        }

        if (cardCost <= tributeEntities.Count)
        {
            if (DataManager.instance.CardData(card_id).cardType.card_category == CardCategory.Monster)
            {
                TributeSummon(isMine, card_id);
            }
            else if (DataManager.instance.CardData(card_id).cardType.card_category == CardCategory.Magic)
            {
                TributeMagic(isMine, card_id);
            }
        }
        else
        {
            foreach (Entity tribute in tributeEntities)
            {
                if (tribute.card.ability.effect_Time == EffectTime.Tribute)
                {
                    if (tribute.card.ability.targetID == card_id)
                    {
                        TributeSummon(isMine, card_id);
                        return;
                    }
                    else
                    {
                        foreach (var effect in tribute.card.ability.effects)
                        {
                            print(effect.effectClass);
                        }

                        if (tribute.card.ability.effects.Exists(x => x.effectClass == EffectClass.tribute))
                        {
                            Effect effect = tribute.card.ability.effects.Find(x => x.effectClass == EffectClass.tribute);
                            tribute_Count = tribute_Count + effect.value;
                        }
                    }
                }
            }

            if (cardCost <= tribute_Count)
            {
                TributeSummon(isMine, card_id);
            }
        }
    }
    #endregion


    #region Magic Card
    public void ConfirmEffectTrigger(string msg = "타겟을\n 선택 하세요")
    {
        GameManager.instance.Notification(msg);
        SelectMonsterMode = true;
    }

    // 지금 텔레포트 전용.. 
    public void Target_Effect_Solver(int entity_Id, Vector3 tilePos, bool isServer)
    {
        bool isMine = GameManager.instance.IsMine(isServer);
        List<Entity> allEntities = new List<Entity>(playerEntities);
        allEntities.AddRange(opponentEntities);

        Entity entity = allEntities.Find(x => x.id == entity_Id);
        Coordinate movePos = new Coordinate(tilePos);

        Tile targetTile;

        if (isMine)
        {
            targetTile = MapManager.instance.mapData[movePos.x, movePos.y];
        }
        else
        {
            movePos.SetReverse(MapManager.instance.mapSize);
            targetTile = MapManager.instance.mapData[movePos.x, movePos.y];
        }

        foreach (Effect effect in effectCard.ability.effects)
        {
            effect.Resolve(this, entity, targetTile);
        }

        //UpdateEntityState();
        effectCard = null;
        MapManager.instance.MapTileInit();
    }

    public GameObject VFX_Instatiate(Vector3 pos)
    {
        GameObject portal = Instantiate(portal_VFX, pos, transform.rotation);
        portal.transform.localScale = new Vector3(5, 5, 5);

        return portal;
    }
    public void Destroy_VFX(GameObject vfx_effect)
    {
        Destroy(vfx_effect);
    }


    public void RandomTargetEffect(int entity_Id, string card_id)
    {
        Entity target_Entity = All_Entities.Find(x => x.id == entity_Id);
        effectManager.ReceiveRandomEffect(target_Entity, card_id); // jsg

        //UpdateEntityState();
    }

    // 몬스터 사망시 처리되는 함수 이름 바꿔야함 entities 로
    public void UpdateEntitiesState()
    {
        List<Entity> destroiedEntities = new List<Entity>();

        foreach (Entity entity in All_Entities)
        {
            if (entity.isDie)
            {
                if (entity.card.ability.effect_Time == EffectTime.Activated)
                {
                    effectManager.ReverseEffect(entity, All_Entities);
                }
                MapManager.instance.mapData[entity.coordinate.x, entity.coordinate.y].onEntity = null;
                MapManager.instance.mapData[entity.coordinate.x, entity.coordinate.y].tileState = TileState.empty;
                destroiedEntities.Add(entity);
                GoToGraveyard(entity);
            }
        }

        foreach (Entity entity in destroiedEntities)
        {
            if (playerEntities.Contains(entity))
            {
                playerEntities.Remove(entity);
            }
            else if (opponentEntities.Contains(entity))
            {
                opponentEntities.Remove(entity);
            }
        }
    }

    public void EntityState(Entity entity)
    {
        if (entity.isDie)
        {
            MapManager.instance.mapData[entity.coordinate.x, entity.coordinate.y].onEntity = null;
            MapManager.instance.mapData[entity.coordinate.x, entity.coordinate.y].tileState = TileState.empty;
            GoToGraveyard(entity);

            if (playerEntities.Contains(entity))
            {
                playerEntities.Remove(entity);
            }
            else if (opponentEntities.Contains(entity))
            {
                opponentEntities.Remove(entity);
            }
        }
    }

    void GoToGraveyard(Entity entity)
    {
        Vector3 gravePosition = entity.isMine ? playerGraveyard.position : oppenentGraveyard.position;

        graveManager.AddGraveCard(entity.card, entity.isMine);

        Sequence sequence = DOTween.Sequence()
                .Append(entity.transform.DOShakePosition(1f))
                .Append(entity.transform.DOMove(gravePosition, 0.5f))
                .OnComplete(() =>
                {
                    entity.transform.DOKill();
                    Destroy(entity.gameObject);
                });
    }

    #endregion

    #region Battle

    public void Attack(int attackerID, int defenderID, bool server)
    {
        bool isMine;
        if (GameManager.instance.MultiMode)
        {
            isMine = NetworkRpcFunc.instance.isServer == server;
        }
        else
        {
            isMine = server;
        }

        List<Entity> allEntities = new List<Entity>(playerEntities);
        allEntities.AddRange(opponentEntities);
        allEntities.AddRange(aiEntities);

        Entity attacker, defender;
        Sequence sequence;

        if (isMine)
        {
            attacker = allEntities.Find(x => x.id == attackerID);
            defender = allEntities.Find(x => x.id == defenderID);
        }
        else
        {
            attacker = allEntities.Find(x => x.id == attackerID);
            defender = allEntities.Find(x => x.id == defenderID);
        }

        int attackerBP = attacker.GetEffectiveValue("bp");
        int defenderBP = defender.GetEffectiveValue("bp");

        if (defender.card.cardType.attack_type == AttackType.shooter)
        {
            attackerBP = attackerBP * 2;
        }

        attacker.GetComponent<Order>().SetMostFrontOrder(true);

        Vector3 attackPosition = attacker.transformPos;

        ResolverEffect(attacker, defender);

        if (attacker.card.cardType.attack_type == AttackType.melee)
        {
            sequence = DOTween.Sequence()
                    .Append(attacker.transform.DOScale(Vector3.one * 1.5f, 0.2f)).SetEase(Ease.InSine)
                    .Append(attacker.transform.DOMove(defender.transformPos, 0.4f)).SetEase(Ease.InSine)
                    .Append(attacker.transform.DOMove(attackPosition, 0.4f).SetEase(Ease.InSine))
                    .Append(attacker.transform.DOScale(Vector3.one, 0.2f)).SetEase(Ease.InSine)
                    .AppendCallback(() =>
                    {
                        GameObject spawnedVFX = Instantiate(explosion_VFX, defender.transformPos, transform.rotation);
                        attacker.Damaged(defenderBP);
                        defender.Damaged(attackerBP);
                        Destroy(spawnedVFX, 5f);
                        //UpdateEntityState();
                    }).OnComplete(() => AttackCallback(attacker, defender));
            
        }
        //else if(attacker.card.cardType.attack_type == AttackType.shooter)
        //{
        //    GameObject bullet = Instantiate(bullet_Obj, attacker.transformPos, transform.rotation);
        //    sequence = DOTween.Sequence()
        //            .Append(bullet.transform.DOMove(defender.transformPos, 0.4f)).SetEase(Ease.InSine)
        //            .AppendCallback(() =>
        //            {
        //                GameObject spawnedVFX = Instantiate(explosion_VFX, defender.transform.position, transform.rotation);
        //                Destroy(spawnedVFX, 5f);
        //            }).OnComplete(() => Destroy(bullet, 1f));

        //    defender.Damaged(attackerBP);
        //}

        // 러너 기능이 사라져서 지금 구현 안되어 있음

        attacker.attackable = false;
        if (attacker.belong != EntityBelong.AI)
        {
            canMoveCount--;
        }
        CountTMP_Update();
    }

    public void OutpostAttack(int attackerID, Coordinate outpostCoord, bool server)
    {
        bool isMine;
        if (GameManager.instance.MultiMode)
        {
            isMine = NetworkRpcFunc.instance.isServer == server;
        }
        else
        {
            isMine = server;
        }
        Sequence sequence;

        //Coordinate outpostCoord = new Coordinate(outpostPos);

        //if (isMine == false)
        //    outpostCoord.SetReverse(MapManager.instance.mapSize);

        Entity attacker;
        Outpost outpost = MapManager.instance.coordinateTile(outpostCoord).outpost;

        attacker = All_Entities.Find(x => x.id == attackerID);
        attacker.GetComponent<Order>().SetMostFrontOrder(true);
        Vector3 attackPosition = attacker.transformPos;

        int attackerBP = attacker.GetEffectiveValue("bp");

        if (attacker.card.cardType.attack_type == AttackType.melee)
        {
            sequence = DOTween.Sequence()
                    .Append(attacker.transform.DOScale(Vector3.one * 1.5f, 0.2f)).SetEase(Ease.InSine)
                    .Append(attacker.transform.DOMove(outpost.transformPos, 0.4f)).SetEase(Ease.InSine)
                    .Append(attacker.transform.DOMove(attackPosition, 0.4f).SetEase(Ease.InSine))
                    .Append(attacker.transform.DOScale(Vector3.one, 0.2f)).SetEase(Ease.InSine)
                    .AppendCallback(() =>
                    {
                        GameObject spawnedVFX = Instantiate(explosion_VFX, outpost.transformPos, transform.rotation);
                        Destroy(spawnedVFX, 5f);
                        outpost.Damaged(attackerBP);
                    }).OnComplete(() => OutpostAttackCallback(attacker, outpost));

        }
        // 원거리 유닛이 현재 사라짐
        //else
        //{
        //    GameObject bullet = Instantiate(bullet_Obj, attacker.transformPos, transform.rotation);

        //    sequence = DOTween.Sequence()
        //            .Append(bullet.transform.DOMove(outpost.transformPos, 0.4f)).SetEase(Ease.InSine)
        //            .AppendCallback(() =>
        //            {
        //                GameObject spawnedVFX = Instantiate(explosion_VFX, outpost.transform.position, transform.rotation);
        //                Destroy(spawnedVFX, 5f);
        //                outpost.Damaged(attackerBP);
        //            }).OnComplete(() =>
        //            {
        //                Destroy(bullet, 1f);
        //                CheckGameResult();
        //            });
        //}
        if (attacker.belong != EntityBelong.AI)
        {
            canMoveCount--;
        }
        CountTMP_Update();
    }

    void AttackCallback(Entity attacker, Entity defender)
    {
        attacker.GetComponent<Order>().SetMostFrontOrder(false);

        if (defender.isDie)
        {
            //UpdateEntityState();
            defender.bottomTile.tileState = TileState.empty;
            if (attacker.isDie == false)
            {
                Sequence sequence = DOTween.Sequence()
                .Append(defender.transform.DOShakePosition(0.5f))
                .Append(defender.transform.DOScale(Vector3.one, 0.3f)).SetEase(Ease.OutCirc)
                //.Append(attacker.transform.DOMove(defender.transformPos, 0.1f).SetEase(Ease.InSine))
                .AppendCallback(() =>
                {
                    if (attacker.card.cardType.attack_type == AttackType.melee)
                    {
                        cardMove.Move(attacker, defender.bottomTile);
                    }
                    else
                    {
                        attacker.MoveTransform(attacker.bottomTile.transformPos, true, 0.5f);
                    }
                });
            }
            else
            {
                attacker.bottomTile.tileState = TileState.empty;
            }
        }
        else
        {
            Sequence sequence = DOTween.Sequence()
                .Append(attacker.transform.DOShakePosition(0.5f))
                .AppendCallback(() =>
                {
                    GameObject deathEffect = Instantiate(death_VFX, defender.transformPos, transform.rotation);

                    if (attacker.card.cardType.attack_type == AttackType.melee)
                    {
                        cardMove.AfterAttackMove(attacker, defender.coordinate); // 공격후 디펜더 바로앞으로 이동시키는것
                    }

                    Destroy(deathEffect, 2f);

                    attacker.MoveTransform(attacker.bottomTile.transformPos, true, 2f);
                });
        }
        attacker.attackable = false;
    }

    void OutpostAttackCallback(Entity attacker, Outpost outpost)
    {
        Tile outpostTile = MapManager.instance.mapData[outpost.coordinate.x, outpost.coordinate.y];

        attacker.GetComponent<Order>().SetMostFrontOrder(false);

        if (outpost.isDie)
        {
            outpostTile.tileState = TileState.empty;

            Sequence sequence = DOTween.Sequence()
                .Append(outpost.transform.DOShakePosition(0.5f))
                .Append(attacker.transform.DOMove(outpost.transformPos, 0.1f).SetEase(Ease.InSine))
                .AppendCallback(() =>
                {
                    if (attacker.card.cardType.attack_type == AttackType.melee)
                    {
                        cardMove.Move(attacker, outpostTile);
                    }

                    MapManager.instance.OutpostDestroy(outpost, attacker.isMine);

                    outpostTile.outpost_object.SetActive(false);
                }).OnComplete(() => CheckGameResult());
        }
        else
        {
            Sequence sequence = DOTween.Sequence()
                .Append(outpost.transform.DOShakePosition(0.5f))
                .AppendCallback(() =>
                {
                    if (attacker.card.cardType.attack_type == AttackType.melee)
                    {
                        cardMove.AfterAttackMove(attacker, outpostTile.coordinate);
                    }

                    attacker.MoveTransform(attacker.bottomTile.transformPos, true, 0.5f);
                }).OnComplete(() => CheckGameResult());
        }
        attacker.attackable = false;
        //UpdateEntityState();
    }

    void ResolverEffect(Entity attacker, Entity defender)
    {
        if (attacker.card.ability.effect_Time == EffectTime.Battle)
        {
            if (attacker.card.ability.target.GetTarget() == EffectTarget.TargetCard)
            {
                foreach (var effect in attacker.card.ability.effects)
                {
                    effect.Resolve(defender);
                }
            }
            else if (attacker.card.ability.target.GetTarget() == EffectTarget.ThisCard)
            {
                foreach (var effect in attacker.card.ability.effects)
                {
                    effect.Resolve(attacker);
                }
            }
        }

        if (defender.card.ability.effect_Time == EffectTime.Battle)
        {
            if (defender.card.ability.target.GetTarget() == EffectTarget.TargetCard)
            {
                foreach (var effect in defender.card.ability.effects)
                {
                    effect.Resolve(attacker);
                }
            }
            else if (defender.card.ability.target.GetTarget() == EffectTarget.ThisCard)
            {
                foreach (var effect in defender.card.ability.effects)
                {
                    effect.Resolve(defender);
                }
            }
        }
    }

    void CheckGameResult()
    {
        if (MapManager.instance.livePlayerOutpost <= 0)
        {
            GameManager.instance.GameResult(false);
        }

        if (MapManager.instance.liveOpponentOutpost <= 0)
        {
            GameManager.instance.GameResult(true);
        }
    }

    #endregion

    // 공격시 조준표시 나오게 하는 함수
    private void ShowTargetPicker(bool isShow)
    {
        if (TurnManager.instance.myTurn == false) { return; }

        TargetPicker.SetActive(isShow);

        if (enermyEntityPick != null)
            TargetPicker.transform.position = enermyEntityPick.transform.position;
        else if (selectTile != null && selectTile.tileState == TileState.enermyOutpost)
            TargetPicker.transform.position = selectTile.transform.position;
    }

    public void AttackableReset(bool isMine)
    {
        List<Entity> targetEntities = isMine ? playerEntities : opponentEntities;
        targetEntities.ForEach(x => x.attackable = true);
    }

}
