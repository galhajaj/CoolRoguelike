using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct Position
{
    public int X, Y;
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static bool operator ==(Position p1, Position p2)
    {
        return (p1.X == p2.X && p1.Y == p2.Y);
    }
    public static bool operator !=(Position p1, Position p2)
    {
        return !(p1.X == p2.X && p1.Y == p2.Y);
    }
    public override bool Equals(object p)
    {
        if (p == null || !(p is Position))
            return false;
        return (X == ((Position)p).X && Y == ((Position)p).Y);
    }
    public override int GetHashCode()
    {
        return X * 1000 + Y;
    }

    public int DistanceTo(Position p)
    {
        return (int)Math.Sqrt(Math.Pow(this.X - p.X, 2) + Math.Pow(this.Y - p.Y, 2));
    }

    public Position Up { get { return new Position(X, Y - 1); } }
    public Position Down { get { return new Position(X, Y + 1); } }
    public Position Right { get { return new Position(X + 1, Y); } }
    public Position Left { get { return new Position(X - 1, Y); } }
    public Position UpRight { get { return new Position(X + 1, Y - 1); } }
    public Position UpLeft { get { return new Position(X - 1, Y - 1); } }
    public Position DownRight { get { return new Position(X + 1, Y + 1); } }
    public Position DownLeft { get { return new Position(X - 1, Y + 1); } }

    // the correlated dungeon tile in dungeon
    public DungeonTile DungeonTile
    {
        get { return Dungeon.Instance.GetTile(this); }
    }

    // null position (int MinValue, int MinValue)
    public static Position NullPosition { get { return new Position(int.MinValue, int.MinValue); } }
}
