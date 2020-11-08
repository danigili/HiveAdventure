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

    private void Update()
    {
        transform.position = GetWorldPosition();
    }

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

    public void SetHexPosition(HexObject obj)
    {
        this.x = obj.x;
        this.y = obj.y;
        this.z = obj.z;
    }

    public Position GetHexPosition()
    {
        return new Position(x, y, z);
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
