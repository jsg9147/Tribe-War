using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTypeEffect : TypeChange
{
    public override void Resolve(Entity entity)
    {
        originType = entity.card.cardType.attack_type;
        entity.card.cardType.attack_type = changeType;

        Debug.Log(entity.card.cardType.attack_type);
    }

    public override void Reverse(Entity entity)
    {
        entity.card.cardType.attack_type = originType;
        base.Reverse(entity);
    }

    public void SetChangeType(string typeStr)
    {
        switch (typeStr)
        {
            case "melee":
                this.changeType = AttackType.melee;
                break;
            case "shooter":
                this.changeType = AttackType.shooter;
                break;

            default:
                break;
        }
        
    }
}
