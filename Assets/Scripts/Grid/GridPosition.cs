using System;

public struct GridPosition : IEquatable<GridPosition>
{
    public int x;
    public int z;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x">当前横向格子</param>
    /// <param name="z">当前纵向格子</param>
    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
                x == position.x &&
                z == position.z;

    }

    public bool Equals(GridPosition other)
    {
        return this == other;
    }


    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }


    /// <summary>
    /// 用于Debug
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"x:{x}; z:{z}";
    }

    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.z == b.z;
    }

    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        GridPosition gridPosition = new(a.x + b.x, a.z + b.z);
        return gridPosition;
    }

    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        GridPosition gridPosition = new(a.x - b.x, a.z - b.z);
        return gridPosition;
    }
}
