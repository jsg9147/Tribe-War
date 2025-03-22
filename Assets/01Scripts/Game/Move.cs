using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DarkTonic.MasterAudio;
public class CardMove
{
    CardPossibleMove cardPossibleMove = new CardPossibleMove();
    List<Coordinate> canMovePositionList;
    List<Coordinate> canAttackPositionList;

    public bool Move(Entity targetEntity, Tile movePointTile)
    {
        if(targetEntity == null) { return false; }

        if (movePointTile.tileState != TileState.empty || targetEntity.isDie) { return false; }

        FindCanMovePositionList(targetEntity);

        if (CanMoveCoordinate(movePointTile.coordinate) == false) { return false; }
        targetEntity.bottomTile.onEntity = null;

        movePointTile.onEntity = targetEntity;
        
        MapManager.instance.mapData[targetEntity.coordinate.x, targetEntity.coordinate.y].tileState = TileState.empty;

        targetEntity.transformPos = movePointTile.transform.position;
        targetEntity.GetComponent<Order>()?.SetOriginOrder(10);

        targetEntity.coordinate = movePointTile.coordinate;
        targetEntity.attackable = false;

        MasterAudio.PlaySound("cardSlide8");
        return true;
        // PlayLogControl.instance.Log_Sorter(LogCategory.Move, targetEntity, beforeCoord);
    }


    public List<Coordinate> FindCanMovePositionList(Entity entity)
    {
        canMovePositionList = new List<Coordinate>();
        switch (entity.card.cardType.moveType)
        {
            case MoveType.Rook:
                canMovePositionList.AddRange(cardPossibleMove.Rook_Distance(entity));
                break;
            case MoveType.Bishop:
                canMovePositionList.AddRange(cardPossibleMove.Bishop_Distance(entity));
                break;
            case MoveType.Queen:
                canMovePositionList.AddRange(cardPossibleMove.Rook_Distance(entity));
                canMovePositionList.AddRange(cardPossibleMove.Bishop_Distance(entity));
                canMovePositionList = canMovePositionList.Distinct().ToList();
                break;

            default:
                break;
        }
        return canMovePositionList;
    }

    public List<Coordinate> Can_Attack_Position(Entity fieldCard)
    {
        canAttackPositionList = new List<Coordinate>();
        switch (fieldCard.card.cardType.moveType)
        {
            case MoveType.Rook:
                canAttackPositionList.AddRange(cardPossibleMove.Rook_Distance(fieldCard, true));
                break;
            case MoveType.Bishop:
                canAttackPositionList.AddRange(cardPossibleMove.Bishop_Distance(fieldCard, true));
                break;
            case MoveType.Queen:
                canAttackPositionList.AddRange(cardPossibleMove.Rook_Distance(fieldCard, true));
                canAttackPositionList.AddRange(cardPossibleMove.Bishop_Distance(fieldCard, true));
                canAttackPositionList = canAttackPositionList.Distinct().ToList();
                break;

            default:
                break;
        }
        return canAttackPositionList;
    }

    bool CanMoveCoordinate(Coordinate coordinate)
    {
        foreach(Coordinate canMove_Coordinate in canMovePositionList)
        {
            if (canMove_Coordinate.vector3Pos == coordinate.vector3Pos)
            {
                return true;
            }
        }
        return false;
    }

    public void AfterAttackMove(Entity attacker, Coordinate defenderCoordinate)
    {
        Coordinate attackerCoordinate = attacker.coordinate;

        Coordinate moveCoordinate = new Coordinate();

        switch (attacker.card.cardType.moveType)
        {
            case MoveType.Rook:
                if (attackerCoordinate.x > defenderCoordinate.x)
                    moveCoordinate = new Coordinate(defenderCoordinate.x + 1, defenderCoordinate.y);

                else if (attackerCoordinate.x < defenderCoordinate.x)
                    moveCoordinate = new Coordinate(defenderCoordinate.x - 1, defenderCoordinate.y);

                else if (attackerCoordinate.y > defenderCoordinate.y)
                    moveCoordinate = new Coordinate(defenderCoordinate.x, defenderCoordinate.y + 1);

                else if (attackerCoordinate.x < defenderCoordinate.y)
                    moveCoordinate = new Coordinate(defenderCoordinate.x, defenderCoordinate.y - 1);

                break;

            case MoveType.Bishop:
                if (attackerCoordinate.x > defenderCoordinate.x)
                {
                    if (attackerCoordinate.y > defenderCoordinate.y)
                        moveCoordinate = new Coordinate(defenderCoordinate.x + 1, defenderCoordinate.y + 1);

                    else if (attackerCoordinate.y < defenderCoordinate.y)
                        moveCoordinate = new Coordinate(defenderCoordinate.x + 1, defenderCoordinate.y - 1);
                }
                else if (attackerCoordinate.x < defenderCoordinate.x)
                {
                    if (attackerCoordinate.y > defenderCoordinate.y)
                        moveCoordinate = new Coordinate(defenderCoordinate.x - 1, defenderCoordinate.y + 1);

                    else if (attackerCoordinate.y < defenderCoordinate.y)
                        moveCoordinate = new Coordinate(defenderCoordinate.x - 1, defenderCoordinate.y - 1);
                }
                break;

            case MoveType.Queen:
                if (attackerCoordinate.x > defenderCoordinate.x)
                {
                    if (attackerCoordinate.y > defenderCoordinate.y)
                        moveCoordinate = new Coordinate(defenderCoordinate.x + 1, defenderCoordinate.y + 1);

                    else if (attackerCoordinate.y < defenderCoordinate.y)
                        moveCoordinate = new Coordinate(defenderCoordinate.x + 1, defenderCoordinate.y - 1);

                    else if (attackerCoordinate.y == defenderCoordinate.y)
                        moveCoordinate = new Coordinate(defenderCoordinate.x + 1, defenderCoordinate.y);
                }
                else if (attackerCoordinate.x < defenderCoordinate.x)
                {
                    if (attackerCoordinate.y > defenderCoordinate.y)
                        moveCoordinate = new Coordinate(defenderCoordinate.x - 1, defenderCoordinate.y + 1);

                    else if (attackerCoordinate.y < defenderCoordinate.y)
                        moveCoordinate = new Coordinate(defenderCoordinate.x - 1, defenderCoordinate.y - 1);

                    else if (attackerCoordinate.y == defenderCoordinate.y)
                        moveCoordinate = new Coordinate(defenderCoordinate.x - 1, defenderCoordinate.y);
                }

                else if (attackerCoordinate.x == defenderCoordinate.x)
                {
                    if (attackerCoordinate.y > defenderCoordinate.y)
                        moveCoordinate = new Coordinate(defenderCoordinate.x, defenderCoordinate.y + 1);

                    else if (attackerCoordinate.y < defenderCoordinate.y)
                        moveCoordinate = new Coordinate(defenderCoordinate.x, defenderCoordinate.y - 1);
                }
                break;
        }

        Tile moveCoordinateTile = MapManager.instance.mapData[moveCoordinate.x, moveCoordinate.y];

        if(attacker.coordinate != moveCoordinateTile.coordinate)
        {
            Move(attacker, moveCoordinateTile);
        }
        else
        {
            Sequence sequence = DOTween.Sequence().Append(attacker.transform.DOMove(moveCoordinateTile.transformPos, 0.5f).SetEase(Ease.InSine));
        }
    }
}
