using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonCountControl : SummonCountEffect
{
    public override void Resolve(EntityManager entityManager, Card card, bool isMine)
    {
        if (entityManager.tribeSummonCount.ContainsKey(card.cardType.tribe))
        {
            entityManager.tribeSummonCount[card.cardType.tribe] = entityManager.tribeSummonCount[card.cardType.tribe] + 1;
        }
        else
        {
            entityManager.tribeSummonCount.Add(card.cardType.tribe, 1);
        }
    }
}
