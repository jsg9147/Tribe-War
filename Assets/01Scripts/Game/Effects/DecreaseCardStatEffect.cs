using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseCardStatEffect : CardStatEffect
{
    public override void Resolve(Card card)
    {
        var modfier = new Modifier(-value, duration);
        card.stats[statID].AddModifier(modfier);
    }

    public override void Resolve(Entity entity)
    {
        var modfier = new Modifier(-value, duration);
        entity.card.stats[statID].AddModifier(modfier);

        entity.UpdateStat();
    }
    public override void Reverse(Entity entity)
    {
        var modfier = new Modifier(value, duration);
        entity.card.stats[statID].AddModifier(modfier);

        entity.UpdateStat();
    }
}
