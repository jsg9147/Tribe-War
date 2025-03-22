using System.Collections;
using System.Collections.Generic;

public abstract class Effect
{
    /// <summary>
    /// Returns true if there are any targets available for this effect and false otherwise.
    /// </summary>
    /// <param name="state">The game's state.</param>
    /// <param name="sourceCard">The card containing the effect.</param>
    /// <param name="target">The target type of the effect.</param>
    /// <returns>True if there are any targets available for this effect; false otherwise.</returns>
    /// 
    public int value;
    public int duration;
    public EffectClass effectClass;

    public virtual bool AreTargetsAvailable(Card sourceCard, Target target)
    {
        return true;
    }
    public virtual bool AreTargetsAvailable(Card tributeCard, Card targetCard)
    {
        return true;
    }
    public virtual void Resolve(Entity entity)
    {
    }

    public virtual void Reverse(Entity entity)
    {

    }
    public virtual void Resolve(EntityManager entityManager, Card card, bool isMine)
    {

    }

    public virtual void Resolve(EntityManager entityManager, Entity entity, Tile targetTile)
    {
    
    }
}

public abstract class CardBaseEffect : Effect
{   
    public virtual void Resolve(Card card)
    {

    }
}

public abstract class CardEffect : CardBaseEffect
{
    public override bool AreTargetsAvailable(Card sourceCard, Target target)
    {
        switch (target.GetTarget())
        {
            case EffectTarget.ThisCard:
                break;

            case EffectTarget.PlayerCard:
                break;

            case EffectTarget.EnermyCard:
                break;

            case EffectTarget.TargetCard:
                break;

            case EffectTarget.RandomPlayerCard:
                break;

            case EffectTarget.RandomCard:
                break;

            case EffectTarget.AllPlayerCards:
                break;

            case EffectTarget.AllOpponentCards:
                break;

            case EffectTarget.AllCards:
                break;
            case EffectTarget.Player:
                break;
            default:
                break;
        }
        return base.AreTargetsAvailable(sourceCard, target);
    }

    public virtual void SetStatID(string stat_name)
    {

    }
}

public abstract class CardStatEffect : CardEffect
{
    /// <summary>
    /// The unique identifier of the stat.
    /// 0 = attack, 1 = defense.
    /// </summary>
    public int statID;

    public override void SetStatID(string stat_name)
    {
        if (stat_name == "bp")
            statID = 0;
        else if (stat_name == "attack")
            statID = 1;
        else if (stat_name == "defense")
            statID = 2;
        else
            return;
    }
}

public abstract class CardDestroyEffect : CardEffect
{
    /// <summary>
    /// The Card destroy
    /// </summary>
    public int cardIndex;
}

public abstract class SummonCountEffect : CardEffect
{
    public int count;
    public Tribe targetTribe;
}

public abstract class CardMoveEffect : CardEffect
{
    public Coordinate moveCoord;

}

public abstract class TributeBaseEffect : CardEffect
{
    public Tribe targetTribe;
}

public abstract class ControllEffect : CardEffect
{
    public EntityBelong originBelong;
    public int turn;
}

public abstract class TypeChange : CardEffect
{
    public AttackType originType;
    public AttackType changeType;
}
