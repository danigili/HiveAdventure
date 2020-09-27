using System.Collections;

[System.Serializable]
public class Position
{
    public int x;
    public int y;
    public int z;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.z = 0;
    }

    public Position(int x, int y, int z) : this(x, y)
    {
        this.z = z;
    }

    public Position((int, int, int) position)
    {
        SetPosition(position);
    }

    public void SetPosition(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public void SetPosition((int, int, int) position)
    {
        this.x = position.Item1;
        this.y = position.Item2;
        this.z = position.Item3;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (obj is Position)
            return this.x == ((Position)obj).x && this.y == ((Position)obj).y;
        return false;
    }

    public override int GetHashCode()
    {
        return ("" + x + "," + y + "," + z).GetHashCode();
    }
}
