using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//텔레포트
public class MoveEffect : CardMoveEffect
{
    public override void Resolve(EntityManager entityManager, Entity entity, Tile targetTile)
    {
        if (entity == null) { return; }
        if (targetTile.onEntity) { return; }

        targetTile.onEntity = entity;

        if (entity.isMine)
        {
            targetTile.tileState = TileState.onPlayerMonster;
        }
        else
        {
            targetTile.tileState = TileState.onEnermyMonster;
        }

        MapManager.instance.mapData[entity.coordinate.x, entity.coordinate.y].tileState = TileState.empty;

        GameObject entityVFX = entityManager.VFX_Instatiate(entity.transformPos);
        GameObject targetVFX = entityManager.VFX_Instatiate(targetTile.transformPos);

        Sequence sequence = DOTween.Sequence()
        .Append(entity.transform.DOScale(Vector3.zero, 1f)).SetEase(Ease.InSine)
        .AppendCallback(() =>
        {
            entity.MoveTransform(targetTile.transform.position, false);
            entity.GetComponent<Order>()?.SetOriginOrder(10);

            Sequence sequence1 = DOTween.Sequence()
                .Append(entity.transform.DOScale(Vector3.one, 1f)).SetEase(Ease.InSine)
                .AppendCallback(() =>
                {
                    entityManager.Destroy_VFX(entityVFX);
                    entityManager.Destroy_VFX(targetVFX);
                });
        });

        entity.coordinate = targetTile.coordinate;
        entity.attackable = false;
        
    }
}
