using System.Collections;
using System.Collections.Generic;

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

    public Position Up { get { return new Position(X, Y - 1); } }
    public Position Down { get { return new Position(X, Y + 1); } }
    public Position Right { get { return new Position(X + 1, Y); } }
    public Position Left { get { return new Position(X - 1, Y); } }
    public Position UpRight { get { return new Position(X + 1, Y - 1); } }
    public Position UpLeft { get { return new Position(X - 1, Y - 1); } }
    public Position DownRight { get { return new Position(X + 1, Y + 1); } }
    public Position DownLeft { get { return new Position(X - 1, Y + 1); } }
}
