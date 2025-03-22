using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseCardStatEffect : CardStatEffect
{
    public override void Resolve(Card card)
    {
        Modifier modfier = new Modifier(value, duration);
        card.stats[statID].AddModifier(modfier);
    }
    public override void Resolve(Entity entity)
    {
        Modifier modfier = new Modifier(value, duration);
        if (value == 0)
        {
            entity.card.stats[statID].AllRemoveModifier();
        }
        else
        {
            entity.card.stats[statID].AddModifier(modfier);
        }
        
        entity.UpdateStat();
    }

    public override void Reverse(Entity entity)
    {
        var modfier = new Modifier(-value, duration);
        entity.card.stats[statID].AddModifier(modfier);

        entity.UpdateStat();
    }
}
