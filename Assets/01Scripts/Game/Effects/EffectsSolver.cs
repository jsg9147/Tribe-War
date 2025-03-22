using System.Collections.Generic;
using UnityEngine;

public class EffectsSolver
{
    //public bool randomPlayer;
    //public int randomEntityIndex = 0;

    //public List<Ability> player_Activated_Abilities = new List<Ability>();
    //public List<Ability> opponent_Activated_Abilities = new List<Ability>();

    //public void NonTargetEffectActive(Card card, List<Entity> playerEntities, List<Entity> opponentEntities, bool server)
    //{
    //    switch (card.ability.target.GetTarget())
    //    {
    //        case EffectTarget.RandomCard:
    //            //GameManager.instance.localGamePlayerScript.CmdRandomTargetAppoint(card.id, server);
    //            break;
    //        case EffectTarget.AllCards:
    //            AllCardsEffectTrigger(card, playerEntities, opponentEntities, server);
    //            break;
    //        case EffectTarget.PlayerWarrior:
    //            Player_Warrior_EffectTrigger(card, playerEntities, server);
    //            break;
    //        default:
    //            Debug.Log("Non Target Func, default..");
    //            break;
    //    }
    //}

    //public void PlayerTargetEffect(EntityManager entityManager, Card effectCard, bool isMine)
    //{
    //    foreach (var effect in effectCard.ability.effects)
    //    {
    //        effect.Resolve(entityManager, effectCard, isMine);
    //    }
    //}


    //void AllCardsEffectTrigger(Card effectCard, List<Entity> playerEntities, List<Entity> opponentEntities, bool server)
    //{
    //    List<Entity> allEntities = new List<Entity>(playerEntities);
    //    allEntities.AddRange(opponentEntities);

    //    foreach (var entity in allEntities)
    //    {
    //        foreach (var effect in effectCard.ability.effects)
    //        {
    //            effect.Resolve(entity);
    //        }
    //    }
    //}

    //void Player_Warrior_EffectTrigger(Card effectCard, List<Entity> playerEntities, bool server)
    //{
    //    foreach (var entity in playerEntities)
    //    {
    //        foreach (var effect in effectCard.ability.effects)
    //        {
    //            if (entity.card.cardType.tribe == Tribe.Warrior)
    //                effect.Resolve(entity);
    //        }
    //    }
    //}

    //public void ReceiveRandomEffect(Entity entity, string card_id)
    //{
    //    Card effectCard = CardDatabase.instance.CardData(card_id);

    //    foreach (var effect in effectCard.ability.effects)
    //    {
    //        effect.Resolve(entity);
    //    }
    //}

    //public bool EffectRequireExist(bool isMine, List<Entity> playerEntities, List<Entity> opponentEntities, Card triggerCard)
    //{
    //    List<Entity> targetEntities = isMine ? playerEntities : opponentEntities;

    //    if (triggerCard.cardType.tribe == Tribe.None)
    //        return true;

    //    if (triggerCard.ability.target.GetTarget() == EffectTarget.Player)
    //        return true;

    //    foreach (var entity in targetEntities)
    //    {
    //        if (entity.card.cardType.tribe == triggerCard.cardType.tribe)
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //public void Activated_Effect(Ability ability, bool isMine, List<Entity> playerEntities, List<Entity> opponentEntities)
    //{
    //    List<Entity> allEntities = new List<Entity>(playerEntities);
    //    allEntities.AddRange(opponentEntities);

    //    foreach (var entity in allEntities)
    //    {
    //        if (ability.target.GetTarget() == EffectTarget.PlayerWarrior)
    //        {
    //            if (entity.card.cardType.tribe == Tribe.Warrior && entity.isMine == isMine)
    //            {
    //                entity.Add_Apply_Effect(ability);
    //            }
    //        }
    //    }
    //}

    //public void Add_Activated_Effect_To_Entity(Entity entity)
    //{
    //    foreach (var ability in player_Activated_Abilities)
    //    {
    //        if (ability.target.GetTarget() == EffectTarget.PlayerWarrior)
    //        {
    //            if (entity.card.cardType.tribe == Tribe.Warrior && entity.isMine == true)
    //            {
    //                entity.Add_Apply_Effect(ability);
    //            }
    //        }
    //    }

    //    foreach (var ability in opponent_Activated_Abilities)
    //    {
    //        if (ability.target.GetTarget() == EffectTarget.PlayerWarrior)
    //        {
    //            if (entity.card.cardType.tribe == Tribe.Warrior && entity.isMine == false)
    //            {
    //                entity.Add_Apply_Effect(ability);
    //            }
    //        }
    //    }
    //}

    //public void ReverseEffect(Entity effectEntity, List<Entity> playerEntities, List<Entity> opponentEntities)
    //{
    //    RemoveActivatedAbilities(effectEntity);

    //    if (effectEntity.card.ability.effect_Time == EffectTime.Activated && effectEntity.isMine)
    //    {
    //        if (effectEntity.card.ability.target.GetTarget() == EffectTarget.PlayerWarrior)
    //        {
    //            EntityListApplyEffectCancel(effectEntity.card, playerEntities, Tribe.Warrior);
    //        }
    //        else if (effectEntity.card.ability.target.GetTarget() == EffectTarget.PlayerDragon)
    //        {
    //            EntityListApplyEffectCancel(effectEntity.card, playerEntities, Tribe.Dragon);
    //        }
    //    }
    //    else
    //    {
    //        if (effectEntity.card.ability.target.GetTarget() == EffectTarget.PlayerWarrior)
    //        {
    //            EntityListApplyEffectCancel(effectEntity.card, opponentEntities, Tribe.Warrior);
    //        }
    //        else if (effectEntity.card.ability.target.GetTarget() == EffectTarget.PlayerDragon)
    //        {
    //            EntityListApplyEffectCancel(effectEntity.card, opponentEntities, Tribe.Dragon);
    //        }
    //    }
    //}

    //void RemoveActivatedAbilities(Entity effectEntity)
    //{
    //    var targetAbilities = effectEntity.isMine ? player_Activated_Abilities : opponent_Activated_Abilities;

    //    Ability removeAbility = targetAbilities.Find(x => x.Tag == effectEntity.card.ability.Tag);

    //    targetAbilities.Remove(removeAbility);
    //}

    //void EntityListApplyEffectCancel(Card effectCard, List<Entity> entities, Tribe tribe)
    //{
    //    foreach (var entity in entities)
    //    {
    //        if (entity.card.cardType.tribe == tribe)
    //        {
    //            foreach (var effect in effectCard.ability.effects)
    //            {
    //                effect.Resolve(entity);
    //            }
    //        }
    //    }
    //}
}
