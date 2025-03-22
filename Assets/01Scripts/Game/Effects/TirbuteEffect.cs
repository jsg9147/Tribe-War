using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirbuteEffect : TributeBaseEffect
{
    public override void Resolve(Entity entity)
    {

    }
}

public class TargetTributeEffect : TributeBaseEffect
{
    public override void Resolve(Entity entity)
    {

    }

    public override bool AreTargetsAvailable(Card tributeCard, Card targetCard)
    {

        return base.AreTargetsAvailable(tributeCard, targetCard);
    }
}

public class TributeMagicEffect : TributeBaseEffect
{
    public override void Resolve(Entity entity)
    {

    }

    public override void Resolve(EntityManager entityManager, Card card, bool isMine)
    {
        if (entityManager.tribeTributeCount.ContainsKey(card.cardType.tribe))
        {
            entityManager.tribeTributeCount[card.cardType.tribe] = entityManager.tribeTributeCount[card.cardType.tribe] + value;
        }
        else
        {
            entityManager.tribeTributeCount.Add(card.cardType.tribe, value);
        }
    }
}
