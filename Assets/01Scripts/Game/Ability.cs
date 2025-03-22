using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// The base ability class.
/// </summary>
public class Ability
{
    public EffectTime effect_Time = EffectTime.None;

    public string Tag = "";

    public List<Effect> effects = new List<Effect>();
    public Target target;

    // 효과를 지정해서 사용하냐는 뜻
    public bool targetEffect = false;

    public string targetID = "";

    public void SetEffectSubject(string effectTargetStr)
    {
        switch (effectTargetStr)
        {
            case "target":
                target = new TargetCard();
                break;
            case "player_card":
                target = new PlayerCard();
                targetEffect = true;
                break;
            case "random":
                target = new RandomCard();
                break;
            case "all":
                target = new AllCards();
                break;
            case "self":
                target = new CardSelf();
                break;
            case "player_warrior":
                target = new PlayerWarrior();
                break;
            case "tribe_target":
                target = new TribeTarget();
                targetEffect = true;
                break;
            case "target_warrior":
                target = new TribeTarget();
                targetEffect = true;
                break;
            case "player":
                target = new PlayerTarget();
                break;
            default:
                target = new NoneEffect();
                break;
        }
    }

    public void SetAbility(string card_id, string effect_List_Str, string effect_subject_Str)
    {
        if (effect_List_Str == null)
            return;

        string[] effectStrArray = effect_List_Str.Split('/');

        foreach (var effect_Str in effectStrArray)
        {
            SetEffect(card_id, effect_Str);
        }
        SetEffectSubject(effect_subject_Str);
    }

    public void SetEffect(string card_id, string effectStr)
    {
        if (effectStr == null || effectStr.Split('.')[0] == "")
            return;

        Tag = effectStr;
        switch (effectStr.Split('.')[0])
        {
            case "destroy":
                effects.Add(new DestroyCardEntityEffect());
                break;
            case "increase":
                var increaseCardStatEffect = new IncreaseCardStatEffect();
                increaseCardStatEffect.effectClass = EffectClass.increase;
                increaseCardStatEffect.SetStatID(effectStr.Split('.')[1]);
                increaseCardStatEffect.value = Int32.Parse(effectStr.Split('.')[2]); ;
                increaseCardStatEffect.duration = Int32.Parse(effectStr.Split('.')[3]); ;
                effects.Add(increaseCardStatEffect);
                break;
            case "decrease":
                var decreaseCardStatEffect = new DecreaseCardStatEffect();
                decreaseCardStatEffect.effectClass = EffectClass.decrease;
                decreaseCardStatEffect.SetStatID(effectStr.Split('.')[1]);
                decreaseCardStatEffect.value = Int32.Parse(effectStr.Split('.')[2]); ;
                decreaseCardStatEffect.duration = Int32.Parse(effectStr.Split('.')[3]); ;
                effects.Add(decreaseCardStatEffect);
                break;
            case "summon":
                var summonCountEffect = new SummonCountControl();
                summonCountEffect.effectClass = EffectClass.summon;
                summonCountEffect.count = Int32.Parse(effectStr.Split('.')[2]); ;
                effects.Add(summonCountEffect);
                break;

            case "move":
                MoveEffect monsterMoveEffect = new MoveEffect();
                monsterMoveEffect.effectClass = EffectClass.move;
                effects.Add(monsterMoveEffect);
                monsterMoveEffect.value = Int32.Parse(effectStr.Split('.')[1]);
                break;

            case "targetTribute":
                var targetTributeEffect = new TargetTributeEffect();
                targetTributeEffect.effectClass = EffectClass.targetTribute;
                targetID = effectStr.Split('.')[1];
                targetTributeEffect.value = Int32.Parse(effectStr.Split('.')[2]);
                effects.Add(targetTributeEffect);
                break;

            case "tribute":
                var tributeEffect = new TirbuteEffect();
                tributeEffect.effectClass = EffectClass.tribute;
                tributeEffect.value = Int32.Parse(effectStr.Split('.')[1]);
                effects.Add(tributeEffect);
                break;

            case "control":
                ControlLossEffect targetControl = new ControlLossEffect();
                targetControl.effectClass = EffectClass.control;
                targetControl.value = Int32.Parse(effectStr.Split('.')[2]);
                effects.Add(targetControl);
                break;

            case "type":
                AttackTypeEffect typeChange = new AttackTypeEffect();
                typeChange.SetChangeType(effectStr.Split('.')[1]);
                typeChange.value = Int32.Parse(effectStr.Split('.')[2]);
                effects.Add(typeChange);
                break;

            case "tributeMagic":
                TributeMagicEffect tributeMagicEffect = new TributeMagicEffect();
                tributeMagicEffect.effectClass = EffectClass.tributeMagic;
                tributeMagicEffect.value = int.Parse(effectStr.Split('.')[1]);
                effects.Add(tributeMagicEffect);
                break;
            default:
                break;
        }
    }
}
public class TriggerAbility : Ability
{
    public TriggerAbility()
    {
        effect_Time = EffectTime.Triggered;
    }
}
public class ActivateAbility : Ability
{
    public ActivateAbility()
    {
        effect_Time = EffectTime.Activated;
    }
}

public class BattleAbility : Ability
{
    public BattleAbility()
    {
        effect_Time = EffectTime.Battle;
    }
}

public class TributeAbility : Ability
{
    public TributeAbility()
    {
        effect_Time = EffectTime.Tribute;
    }
}
