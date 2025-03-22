using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public EntityManager entityManager;
    public GameObject HandArea;
    public List<Entity> tribute_Entities;

    Card triggered_EffectCard;

    public bool effect_Activated;
    bool moveEffect;

    // effectSover 에 존재하던 변수
    public bool randomPlayer;
    public int randomEntityIndex = 0;

    public List<Ability> player_Activated_Abilities;
    public List<Ability> enermy_Activated_Abilities;
    public List<Ability> ai_Abilities;
    //
    [SerializeField] GameObject damage_VFX;
    [SerializeField] GameObject buff_VFX;
    [SerializeField] GameObject curse_VFX;

    void Start()
    {
        Init();
    }

    void Init()
    {
        player_Activated_Abilities = new List<Ability>();
        enermy_Activated_Abilities = new List<Ability>();
        ai_Abilities = new List<Ability>();

        tribute_Entities = new List<Entity>();

        effect_Activated = false;
    }

    public void Select_Effect_Target(Entity entity)
    {
        if (effect_Activated)
        {
            if (GameManager.instance.MultiMode)
            {
                GameManager.instance.localGamePlayerScript.CmdSelectEffectTarget(entity.id, NetworkRpcFunc.instance.isServer);
            }
            else
            {
                Select_Target(entity.id, true);
            }
        }
    }

    public void Select_Target(int entityID, bool server)
    {
        bool isMine = GameManager.instance.IsMine(server);

        Entity targetEntity = entityManager.All_Entities.Find(x => x.id == entityID);

        if (targetEntity == null)
            return;

        EffectTarget effectTarget = triggered_EffectCard.ability.target.GetTarget();
        switch (effectTarget)
        {
            case EffectTarget.PlayerCard:
                if (targetEntity.belong == EntityBelong.Player)
                {
                    effect_Activated = false;
                }
                break;

            case EffectTarget.EnermyCard:
                if (targetEntity.belong == EntityBelong.Enermy)
                {
                    effect_Activated = false;
                }
                break;

            case EffectTarget.TargetCard:
                {
                    effect_Activated = false;
                }
                break;

            case EffectTarget.TribeTarget:
                if (targetEntity.card.cardType.tribe == triggered_EffectCard.cardType.tribe)
                {
                    effect_Activated = false;
                }
                else
                {
                    GameManager.instance.Notification( DataManager.instance.tribeStr[triggered_EffectCard.cardType.tribe] + LocalizationManager.instance.GetIngameText("PickTarget"));
                }
                break;

            case EffectTarget.Tile:
                break;

            default:
                break;
        }

        if (effect_Activated == false)
        {

#if UNITY_EDITOR
            Debug.Log(triggered_EffectCard.name + " : " + triggered_EffectCard.ability.effects.Count);
#endif
            if (moveEffect && isMine)
            {
                entityManager.clickBlock = true;
                MapManager.instance.SelectMode(targetEntity, triggered_EffectCard.ability);
            }
            else
            {
                foreach (Effect effect in triggered_EffectCard.ability.effects)
                {
                    effect.Resolve(targetEntity);

                    EffectVFX_Instantiate(targetEntity, effect);
                }
            }
            entityManager.effect_Count++;
        }
        //entityManager.UpdateEntityState();
    }

    void EffectVFX_Instantiate(Entity entity, Effect effect)
    {
        GameObject effectVFX;
        switch (effect.effectClass)
        {
            case EffectClass.increase:
                effectVFX = Instantiate(buff_VFX, entity.transformPos, transform.rotation) as GameObject;
                Destroy(effectVFX, 3f);
                break;

            case EffectClass.decrease:
                if (effect.duration == 0)
                {
                    effectVFX = Instantiate(damage_VFX, entity.transformPos, transform.rotation) as GameObject;
                    Destroy(effectVFX, 3f);
                }
                else
                {
                    effectVFX = Instantiate(curse_VFX, entity.transformPos, transform.rotation) as GameObject;
                    Destroy(effectVFX, 3f);
                }
                break;
            default:
                break;
        }
    }


    public bool EffectTrigger(bool isMine, string card_id)
    {
        triggered_EffectCard = DataManager.instance.CardData(card_id);
        if (EffectRequireExist(isMine, entityManager.All_Entities, triggered_EffectCard) == false)
        {
            return false;
        }

        if (triggered_EffectCard.ability.effect_Time == EffectTime.Activated)
        {
            if (isMine)
            {
                player_Activated_Abilities.Add(triggered_EffectCard.ability);
            }
            else
            {
                enermy_Activated_Abilities.Add(triggered_EffectCard.ability);
            }
            Activated_Effect(triggered_EffectCard.ability, isMine, entityManager.All_Entities);
            return true;
        }
        else if (triggered_EffectCard.ability.effect_Time == EffectTime.Triggered)
        {
            if (isMine)
            {
                ModeEffectSolve(isMine);
            }
            return true;
        }
        if (triggered_EffectCard.cost == 0 || triggered_EffectCard.cardType.card_category == CardCategory.Monster)
        {
            if (isMine)
            {
                ModeEffectSolve(isMine);
            }
            return true;
        }
        else if (triggered_EffectCard.cost > 0)
        {
            if (triggered_EffectCard.cost <= entityManager.tributeEntities.Count)
            {
                ModeEffectSolve(isMine);
                return true;
            }

            if (triggered_EffectCard.cost <= entityManager.playerEntities.Count && isMine)
            {
                entityManager.Select_Monster(false);
            }
        }
        return false;
    }

    void ModeEffectSolve(bool isMine)
    {
        if (GameManager.instance.MultiMode)
        {
            GameManager.instance.localGamePlayerScript.CmdEffectSolve(triggered_EffectCard.id, NetworkRpcFunc.instance.isServer);
        }
        else
        {
            EffectSolve(triggered_EffectCard.id, isMine);
        }
    }


    public void EffectSolve(string card_id, bool server)
    {
        bool isMine = GameManager.instance.IsMine(server);
        Card effectCard = DataManager.instance.CardData(card_id);
        List<Entity> all_Entities = entityManager.All_Entities;

        if (effectCard.ability == null) { return; }

        switch (effectCard.ability.target.GetTarget())
        {
            case EffectTarget.TargetCard:
            case EffectTarget.PlayerCard:
                if (isMine)
                {
                    if (effectCard.cardType.card_category == CardCategory.Monster)
                    {
                        entityManager.Select_Monster(false);
                    }
                    else
                    {
                        if (effectCard.ability.Tag.Contains("move"))
                        {
                            entityManager.effectCard = effectCard;
                            moveEffect = true;
                        }

                        GameManager.instance.Notification(LocalizationManager.instance.GetIngameText("PickTarget"));
                        effect_Activated = true;
                    }
                }
                else
                {
                    if (effectCard.ability.Tag.Contains("move"))
                    {
                        entityManager.effectCard = effectCard;
                        moveEffect = true;
                    }
                    effect_Activated = true;
                }
                break;
            case EffectTarget.RandomCard:
                if (all_Entities.Count != 0)
                {
                    if (isMine) // 둘중 한명만 동작해라는 뜻 같음
                    {
                        NonTargetEffectActive(effectCard, all_Entities, server);
                    }
                }
                break;

            case EffectTarget.AllCards:
                NonTargetEffectActive(effectCard, all_Entities, server);
                break;

            case EffectTarget.TribeTarget:
                if (isMine)
                {
                    if (effectCard.cardType.card_category == CardCategory.Monster)
                    {
                        entityManager.Select_Monster(false);
                    }
                    else
                    {

                        if (effectCard.ability.Tag.Contains("move"))
                        {
                            moveEffect = true;
                        }

                        GameManager.instance.Notification(LocalizationManager.instance.GetIngameText("PickTarget"));
                        effect_Activated = true;
                    }
                }
                break;
            case EffectTarget.PlayerWarrior:
                NonTargetEffectActive(effectCard, all_Entities, server);
                break;
            case EffectTarget.Player:
                PlayerTargetEffect(effectCard, isMine);
                break;
            default:
                return;
        }

        //entityManager.UpdateEntityState();
    }


    #region EffectSolver 소속

    List<Entity> Target_Entities(List<Entity> all_Entities, EntityBelong belong)
    {
        return all_Entities.FindAll(x => x.belong == belong);
    }

    Tribe Target_Tribe(EffectTarget effectTarget)
    {
        Tribe tribe = Tribe.Common;
        switch (effectTarget)
        {
            case EffectTarget.PlayerWarrior:
            case EffectTarget.OpponentWarrior:
                tribe = Tribe.Warrior;
                break;

            case EffectTarget.PlayerDragon:
            case EffectTarget.OpponentDragon:
                tribe = Tribe.Dragon;
                break;

            case EffectTarget.PlayerMagician:
            case EffectTarget.OpponentMagician:
                tribe = Tribe.Magician;
                break;

        }
        return tribe;
    
    }

    public void NonTargetEffectActive(Card card, List<Entity> all_Entities, bool server)
    {
        switch (card.ability.target.GetTarget())
        {
            case EffectTarget.RandomCard:
                RandomTargetAppoint(all_Entities, card.id);
                break;
            case EffectTarget.AllCards:
                AllCardsEffectTrigger(card, all_Entities);
                break;
            case EffectTarget.PlayerWarrior:
                Player_Warrior_EffectTrigger(card, all_Entities);
                break;
            default:
                Debug.Log("Non Target Func, default..");
                break;
        }
    }

    void RandomTargetAppoint(List<Entity> all_Entities, string card_id)
    {
        int targetIndex = Random.Range(0, all_Entities.Count - 1);

        if (GameManager.instance.MultiMode)
        {
            GameManager.instance.localGamePlayerScript.CmdRandomTargetAppoint(all_Entities[targetIndex].id, card_id);
        }
        else
        {
            EntityManager.instance.RandomTargetEffect(all_Entities[targetIndex].id, card_id);
        }
    }

    public void PlayerTargetEffect(Card effectCard, bool isMine)
    {
        foreach (Effect effect in effectCard.ability.effects)
        {
            effect.Resolve(entityManager, effectCard, isMine);
        }
    }


    void AllCardsEffectTrigger(Card effectCard, List<Entity> all_Entities)
    {
        foreach (Entity entity in all_Entities)
        {
            foreach (Effect effect in effectCard.ability.effects)
            {
                effect.Resolve(entity);
            }
        }
    }

    void Player_Warrior_EffectTrigger(Card effectCard, List<Entity> all_Entities)
    {
        foreach (var entity in all_Entities)
        {
            if (entity.belong == EntityBelong.Player)
            {
                foreach (var effect in effectCard.ability.effects)
                {
                    if (entity.card.cardType.tribe == Tribe.Warrior)
                    {
                        effect.Resolve(entity);
                    }
                }
            }
        }
    }

    public void ReceiveRandomEffect(Entity entity, string card_id)
    {
        Card effectCard = DataManager.instance.CardData(card_id);

        foreach (Effect effect in effectCard.ability.effects)
        {
            effect.Resolve(entity);
            EffectVFX_Instantiate(entity, effect);
        }
    }

    public bool EffectRequireExist(bool isMine, List<Entity> all_Entities, Card triggerCard)
    {
        EntityBelong belong = isMine ? EntityBelong.Player : EntityBelong.Enermy;
        List<Entity> target_Entities = all_Entities.FindAll(x => x.belong == belong);

        if (triggerCard.cardType.tribe == Tribe.Common)
            return true;

        if (triggerCard.ability.target.GetTarget() == EffectTarget.Player)
            return true;

        foreach (Entity entity in target_Entities)
        {
            if (entity.card.cardType.tribe == triggerCard.cardType.tribe)
            {
                return true;
            }
        }

        return false;
    }

    public void Activated_Effect(Ability ability, bool isMine, List<Entity> all_Entities)
    {
        foreach (Entity entity in all_Entities)
        {
            if (ability.target.GetTarget() == EffectTarget.PlayerWarrior)
            {
                if (isMine)
                {
                    if (entity.card.cardType.tribe == Tribe.Warrior && entity.belong == EntityBelong.Player)
                    {
                        entity.Add_Apply_Effect(ability);
                    }
                }
                else
                {
                    if (entity.card.cardType.tribe == Tribe.Warrior && entity.belong == EntityBelong.Enermy)
                    {
                        entity.Add_Apply_Effect(ability);
                    }
                }
            }
        }
    }

    public void Add_Activated_Effect_To_Entity(Entity entity)
    {
        List<Ability> abilities = entity.isMine ? player_Activated_Abilities : enermy_Activated_Abilities;
        EntityBelong entityBelong = entity.isMine ? EntityBelong.Player : EntityBelong.Enermy;

        foreach (Ability ability in abilities)
        {
            if (ability.target.GetTarget() == EffectTarget.PlayerWarrior)
            {
                if (entity.card.cardType.tribe == Tribe.Warrior && entity.belong == entityBelong)
                {
                    entity.Add_Apply_Effect(ability);
                }
            }
        }
    }

    public void ReverseEffect(Entity effectEntity, List<Entity> all_Entities)
    {
        List<Ability> target_Abilities;

        if (effectEntity.belong == EntityBelong.Player)
        {
            target_Abilities = player_Activated_Abilities;
        }
        else if (effectEntity.belong == EntityBelong.Enermy)
        {
            target_Abilities = enermy_Activated_Abilities;
        }
        else
        {
            target_Abilities = ai_Abilities;
        }

        Ability removeAbility = target_Abilities.Find(x => x.Tag == effectEntity.card.ability.Tag);

        target_Abilities.Remove(removeAbility);

        if (effectEntity.card.ability.effect_Time == EffectTime.Activated)
        {
            Tribe tribe = Target_Tribe(effectEntity.card.ability.target.GetTarget());

            EffectRemove(effectEntity.card, Target_Entities(all_Entities, effectEntity.belong), tribe);
        }
    }


    void EffectRemove(Card effectCard, List<Entity> entities, Tribe tribe)
    {
        foreach (Entity entity in entities)
        {
            if (entity.card.cardType.tribe == tribe)
            {
                foreach (Effect effect in effectCard.ability.effects)
                {
                    effect.Resolve(entity);
                }
            }
        }
    }

    #endregion
}
