using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct Position
{
    public int X;
    public int Y;
    public int Z;

    public Position(int x, int y, int z = 0)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static bool operator ==(Position p1, Position p2)
    {
        return (p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z);
    }
    public static bool operator !=(Position p1, Position p2)
    {
        return !(p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z);
    }
    public override bool Equals(object p)
    {
        if (p == null || !(p is Position))
            return false;
        return (X == ((Position)p).X && Y == ((Position)p).Y && Z == ((Position)p).Z);
    }
    public override int GetHashCode()
    {
        return X * 1000000 + Y * 1000 + Z;
    }
    public override string ToString()
    {
        return string.Format("[{0},{1},{2}]", X, Y, Z);
    }

    public int DistanceTo(Position p)
    {
        return (int)Math.Sqrt(Math.Pow(this.X - p.X, 2) + Math.Pow(this.Y - p.Y, 2) + Math.Pow(this.Z - p.Z, 2));
    }

    public Position North { get { return new Position(X, Y - 1, Z); } }
    public Position South { get { return new Position(X, Y + 1, Z); } }
    public Position East { get { return new Position(X + 1, Y, Z); } }
    public Position West { get { return new Position(X - 1, Y, Z); } }
    public Position NorthEast { get { return new Position(X + 1, Y - 1, Z); } }
    public Position NorthWest { get { return new Position(X - 1, Y - 1, Z); } }
    public Position SouthEast { get { return new Position(X + 1, Y + 1, Z); } }
    public Position SouthWest { get { return new Position(X - 1, Y + 1, Z); } }
    public Position Up { get { return new Position(X, Y, Z + 1); } }
    public Position Down { get { return new Position(X, Y, Z - 1); } }
    public Position Origin { get { return new Position(0, 0, 0); } }
    public Position None { get { return this; } }

    public Position GetPositionInDirection(Direction direction)
    {
        if (direction == Direction.NORTH)
            return North;
        if (direction == Direction.SOUTH)
            return South;
        if (direction == Direction.WEST)
            return West;
        if (direction == Direction.EAST)
            return East;
        if (direction == Direction.UP)
            return Up;
        if (direction == Direction.DOWN)
            return Down;
        if (direction == Direction.NORTH_EAST)
            return NorthEast;
        if (direction == Direction.SOUTH_EAST)
            return SouthEast;
        if (direction == Direction.NORTH_WEST)
            return NorthWest;
        if (direction == Direction.SOUTH_WEST)
            return SouthWest;
        if (direction == Direction.NONE)
            return None;
        if (direction == Direction.ORIGIN)
            return Origin;

        return this;
    }

    public bool IsOutsideDungeonArea
    {
        get { return !(X >= 0 && X < Consts.DUNGEON_AREA_WIDTH && Y >= 0 && Y < Consts.DUNGEON_AREA_HEIGHT); }
    }

    public Position CyclicPosition
    {
        get
        {
            int x = X;
            int y = Y;
            if (x < 0) x = Consts.DUNGEON_AREA_WIDTH - 1;
            if (x >= Consts.DUNGEON_AREA_WIDTH) x = 0;
            if (y < 0) y = Consts.DUNGEON_AREA_HEIGHT - 1;
            if (y >= Consts.DUNGEON_AREA_HEIGHT) y = 0;
            return new Position(x, y);
        }
    }

    // the correlated dungeon tile in dungeon
    public DungeonTile DungeonTile
    {
        get { return Dungeon.Instance.GetTile(this); }
    }

    // null position (int MinValue, int MinValue)
    public static Position NullPosition { get { return new Position(int.MinValue, int.MinValue, int.MinValue); } }

    // origin position (0, 0, 0)
    public static Position OriginPosition { get { return new Position(0, 0, 0); } }
}
