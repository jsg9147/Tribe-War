using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PRS
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

    public PRS(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
    }
}

public class Utils
{
    public static Quaternion QI => Quaternion.identity;

    public static Vector3 MousePos
    {
        get
        {
            Vector3 result = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            result.z = -10;
            return result;
        }
    }

}

// 좌표값
public class Coordinate
{
    public int x, y;
    public Vector3 vector3Pos;
    public Coordinate reverse;
    public int mapSize;

    public Coordinate()
    {

    }
    public Coordinate(Vector2 coordVector)
    {
        this.x = (int)coordVector.x;
        this.y = (int)coordVector.y;
        this.vector3Pos = coordVector;
    }

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.vector3Pos = new Vector2(x, y);
    }

    public void SetReverse(int maxSize)
    {
        mapSize = maxSize - 1;
        int reverseX, reverseY;
        reverseX = mapSize - x;
        reverseY = mapSize - y;

        x = reverseX;
        y = reverseY;
        vector3Pos = new Vector2(reverseX, reverseY);
    }

    public bool MaxDistance(Coordinate coord, int distance)
    {
        int distanceX = Mathf.Abs(coord.x - x);
        int distanceY = Mathf.Abs(coord.y - y);

        return distanceX + distanceY <= distance;
    }
}


