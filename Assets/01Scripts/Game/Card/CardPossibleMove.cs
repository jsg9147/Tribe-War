using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPossibleMove
{
    int CurrentX, CurrentY;
    int mapSize;
    List<Coordinate> canMovePointList;


    bool Can_Attack(Tile directTile, Entity entity, Coordinate coordinate)
    {
        if (directTile.isEmpty)
        {
            canMovePointList.Add(coordinate);
            return false;
        }

        if (directTile.onTarget != EntityBelong.None)
        {
            if (directTile.onTarget == entity.belong)
            {
                return true;
            }
            else if (directTile.onTarget != entity.belong)
            {
                canMovePointList.Add(coordinate);
                return true;
            }
        }

        if (directTile.outpost.isActive)
        {
            
            if (directTile.outpost.belong == entity.belong)
            {
                return true;
            }

            else if (directTile.outpost.belong != entity.belong)
            {
                canMovePointList.Add(coordinate);
                return true;
            }
        }
        

        return false;
    }

    public List<Coordinate> Rook_Distance(Entity entity, bool attack = false)
    {
        int moveSpace;
        mapSize = MapManager.instance.mapSize;
        Tile[,] mapData = MapManager.instance.mapData;

        canMovePointList = new List<Coordinate>();

        CurrentX = entity.coordinate.x;
        CurrentY = entity.coordinate.y;

        moveSpace = entity.card.cost + 1;

        if (!attack && entity.card.cardType.attack_type == AttackType.shooter)
        {
            moveSpace = 1;
        }

        int i = CurrentX + 1;

        // Right        

        while (true)
        {
            if (i >= mapSize || i - CurrentX > moveSpace)
                break;

            Tile rightDir = mapData[i, CurrentY];
            if (Can_Attack(rightDir, entity, new Coordinate(i, CurrentY)))
            {
                break;
            }

            i++;
        }

        // Left        
        i = CurrentX - 1;
        while (true)
        {
            if (i < 0 || CurrentX - i > moveSpace)
                break;

            Tile leftDir = mapData[i, CurrentY];

            if (Can_Attack(leftDir, entity, new Coordinate(i, CurrentY)))
            {
                break;
            }

            i--;
        }

        // Up        
        i = CurrentY + 1;
        while (true)
        {
            if (i >= mapSize || i - CurrentY > moveSpace)
                break;

            Tile upDir = mapData[CurrentX, i];

            if (Can_Attack(upDir, entity, new Coordinate(CurrentX, i)))
            {
                break;
            }

            i++;
        }

        // Down
        i = CurrentY - 1;
        while (true)
        {
            if (i < 0 || CurrentY - i > moveSpace)
                break;

            Tile downDir = mapData[CurrentX, i];

            if (Can_Attack(downDir, entity, new Coordinate(CurrentX, i)))
            {
                break;
            }

            i--;
        }
        return canMovePointList;
    }

    public List<Coordinate> Bishop_Distance(Entity entity, bool attack = false)
    {
        mapSize = MapManager.instance.mapSize;
        Tile[,] mapData = MapManager.instance.mapData;

        canMovePointList = new List<Coordinate>();

        CurrentX = entity.coordinate.x;
        CurrentY = entity.coordinate.y;

        int i = CurrentX + 1;
        int j = CurrentY + 1;

        int moveSpace = (entity.card.cardType.moveType == MoveType.Queen) ? entity.card.cost : entity.card.cost + 1;

        if (!attack && entity.card.cardType.attack_type == AttackType.shooter)
        {
            moveSpace = 1;
        }

        // Right Up
        while (true)
        {
            if (i >= mapSize || j >= mapSize)
                break;

            if (i - CurrentX > moveSpace || j - CurrentY > moveSpace)
                break;

            Tile rightUp = mapData[i, j];

            //if (rightUp.isEmpty)
            //{
            //    canMovePointList.Add(new Coordinate(i, j));
            //}
            //else if(rightUp.onTarget != entity.belong)
            //{
            //    canMovePointList.Add(new Coordinate(i, j));
            //    break;
            //}
            //else if (rightUp.outpost.belong != entity.belong)
            //{
            //    canMovePointList.Add(new Coordinate(i, j));
            //    break;
            //}

            if (Can_Attack(rightUp, entity, new Coordinate(i, j)))
            {
                break;
            }

            i++;
            j++;
        }

        // Left Up

        i = CurrentX - 1;
        j = CurrentY + 1;

        while (true)
        {
            if (i < 0 || j >= mapSize)
                break;

            if (CurrentX - i > moveSpace || j - CurrentY > moveSpace)
                break;

            Tile leftUp = mapData[i, j];

            //if (leftUp.isEmpty)
            //{
            //    canMovePointList.Add(new Coordinate(i, j));
            //}
            //else if (leftUp.onTarget != entity.belong)
            //{
            //    canMovePointList.Add(new Coordinate(i, j));
            //    break;
            //}
            //else if (leftUp.outpost.belong != entity.belong)
            //{
            //    canMovePointList.Add(new Coordinate(i, j));
            //    break;
            //}

            if (Can_Attack(leftUp, entity, new Coordinate(i, j)))
            {
                break;
            }

            i--;
            j++;
        }

        // Right Down

        i = CurrentX + 1;
        j = CurrentY - 1;

        while (true)
        {
            if (i >= mapSize || j < 0)
                break;

            if (i - CurrentX > moveSpace || CurrentY - j > moveSpace)
                break;

            Tile rightDown = mapData[i, j];

            //if (rightDown.isEmpty)
            //{
            //    canMovePointList.Add(new Coordinate(i, j));
            //}
            //else if(rightDown.onTarget != entity.belong)
            //{
            //    canMovePointList.Add(new Coordinate(i, j));
            //    break;
            //}
            //else if (rightDown.outpost.belong != entity.belong)
            //{
            //    canMovePointList.Add(new Coordinate(i, j));
            //    break;
            //}

            if (Can_Attack(rightDown, entity, new Coordinate(i, j)))
            {
                break;
            }

            i++;
            j--;
        }

        // Left Down

        i = CurrentX - 1;
        j = CurrentY - 1;

        while (true)
        {
            if (i < 0 || j < 0)
                break;

            if (CurrentX - i > moveSpace || CurrentY - j > moveSpace)
                break;

            Tile leftDown = mapData[i, j];

            //if (leftDown.isEmpty)
            //{
            //    canMovePointList.Add(new Coordinate(i, j));
            //}
            //else if(leftDown.onTarget != entity.belong)
            //{
            //    canMovePointList.Add(new Coordinate(i, j));
            //    break;
            //}
            //else if (leftDown.outpost.belong != entity.belong)
            //{
            //    canMovePointList.Add(new Coordinate(i, j));
            //    break;
            //}

            if (Can_Attack(leftDown, entity, new Coordinate(i, j)))
            {
                break;
            }

            i--;
            j--;
        }
        return canMovePointList;
    }

}
