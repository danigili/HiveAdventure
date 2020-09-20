using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Position
{
    public int x;
    public int y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (obj is Position)
            return this.x == ((Position)obj).x && this.y == ((Position)obj).y;
        return false;
    }
}
