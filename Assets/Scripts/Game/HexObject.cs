using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class HexObject : MonoBehaviour
{
    public int x;
    public int y;
    public int z;
    public static float distance = 1.8f;
    public static float height = 0.65f;

    public void SetHexPosition(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public void SetHexPosition(Position pos)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
    }

    public Vector3 GetWorldPosition()
    {
        Vector3 pos = new Vector3();
        pos.x = distance * 1.73205f * x;
        pos.y = height * z;
        pos.z = distance * y / 2;
        if (y % 2 == 0)
            pos.x += distance * 1.73205f / 2;
        return pos;
    }
}
