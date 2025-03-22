using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// The base class for targets.
/// </summary>
public abstract class Target
{
    public virtual EffectTarget GetTarget()
    {
        return EffectTarget.Player;
    }
}

public interface IPlayerTarget
{
}

public interface ICardTarget
{
}

public interface IUserTarget
{
}

public interface IComputedTarget
{
}

public abstract class PlayerTargetBase : Target, IPlayerTarget
{
    // public List<PlayerCondition> conditions = new List<PlayerCondition>();
}

public abstract class CardTargetBase : Target, ICardTarget
{
    // public List<CardCondition> conditions = new List<CardCondition>();
}

public class PlayerTarget : PlayerTargetBase, IComputedTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.Player;
    }
}

public class OpponentTarget : PlayerTargetBase, IComputedTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.Opponent;
    }
}

public class TargetPlayer : PlayerTargetBase, IUserTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.TargetPlayer;
    }
}

public class RandomPlayer : PlayerTargetBase, IComputedTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.RandomPlayer;
    }
}

public class AllPlayers : PlayerTargetBase, IComputedTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.AllPlayers;
    }
}

public class CardSelf : CardTargetBase, IComputedTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.ThisCard;
    }
}

public class PlayerCard : CardTargetBase, IUserTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.PlayerCard;
    }
}

public class OpponentCard : CardTargetBase, IUserTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.EnermyCard;
    }
}

public class TargetCard : CardTargetBase, IUserTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.TargetCard;
    }
}

public class RandomPlayerCard : CardTargetBase, IComputedTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.RandomPlayerCard;
    }
}

public class RandomOpponentCard : CardTargetBase, IComputedTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.RandomOpponentCard;
    }
}

public class RandomCard : CardTargetBase, IComputedTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.RandomCard;
    }
}

public class AllPlayerCards : CardTargetBase, IComputedTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.AllPlayerCards;
    }
}

public class AllOpponentCards : CardTargetBase, IComputedTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.AllOpponentCards;
    }
}

public class AllCards : CardTargetBase, IComputedTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.AllCards;
    }
}

public class PlayerWarrior : CardTargetBase, IComputedTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.PlayerWarrior;
    }
}

public class TribeTarget : CardTargetBase, IComputedTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.TribeTarget;
    }
}

public class TargetWarrior : CardTargetBase, IComputedTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.TargetWarrior;
    }
}

public class NoneEffect : CardTargetBase, IComputedTarget
{
    public override EffectTarget GetTarget()
    {
        return EffectTarget.None;
    }
}
