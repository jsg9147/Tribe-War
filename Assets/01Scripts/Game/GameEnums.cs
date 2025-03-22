using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityBelong
{
    Player,
    Enermy,
    AI,
    None
}

public enum LogCategory
{
    Summon,
    Move,
    Magic,
    Effected,
    Sacrifice,
    Drop,
    Attack,
    Defend,
    Outpost_Attack
}
public enum EffectTime : int
{
    Triggered,
    Activated,
    Battle,
    Tribute,
    None
}

public enum Timing
{
    Magic, Common, Summon, Battle
}

public enum EcardState
{
    Nothing, CanMouseOver, CanMouseDrag
}

public enum MoveType
{
    Pawn, Rook, Bishop, Knight, Queen, King, None
}

public enum CardCategory
{
    Monster, Magic, Trap
}

public enum AttackType
{
    melee,
    shooter,
    runner
}

public enum Tribe
{
    Dragon, Magician, Warrior, Common
}

public enum EffectTarget
{
    Player,
    Opponent,
    TargetPlayer,
    RandomPlayer,
    AllPlayers,
    ThisCard,
    PlayerCard,
    EnermyCard,
    TargetCard,
    RandomPlayerCard,
    RandomOpponentCard,
    RandomCard,
    AllPlayerCards,
    AllOpponentCards,
    AllCards,
    PlayerWarrior,
    OpponentWarrior,
    PlayerMagician,
    OpponentMagician,
    PlayerDragon,
    OpponentDragon,
    TribeTarget,
    TargetWarrior,
    Tile,
    None
}

public enum TileState
{
    onPlayerMonster,
    onEnermyMonster,
    playerOutpost,
    enermyOutpost, // 추후 삭제 하고 outpost 를 entity 화 시켜야함 지금처럼 같이 두지 말고
    empty,
    AIMonster,
    None
}

public enum CanSpawn
{
    playerCanSpawn,
    opponentCanSpawn,
    all,
    nothing
}

public enum EffectClass
{
    increase,
    decrease,
    destroy,
    move,
    summon,
    tribute,
    targetTribute,
    control,
    tributeMagic
}


public enum Language
{
    KOREA,
    ENGLISH
}