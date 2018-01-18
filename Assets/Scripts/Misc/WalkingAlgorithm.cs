using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingAlgorithm
{
    public static Position GetNextPosition(Position origin, Position destination)
    {
        // TODO: implement
        // need to delete this stupid function
        return getNextStupidTile(origin, destination);
    }

    public static List<Dungeon> GetPath(Position origin, Position destination)
    {
        // TODO: implement
        return null;
    }
    // ====================================================================================== //
    // ====================================================================================== //
    // ====================================================================================== //
    // stupid path - to be deleted! :-)
    // ====================================================================================== //
    // ====================================================================================== //
    // ====================================================================================== //
    private static Position getNextStupidTile(Position from, Position to)
    {
        Position nextPos = new Position(-1, -1);
        int minDistance = 10000;

        helper4GetNextStupidTile(from.Up, to, ref nextPos, ref minDistance);
        helper4GetNextStupidTile(from.Down, to, ref nextPos, ref minDistance);
        helper4GetNextStupidTile(from.Right, to, ref nextPos, ref minDistance);
        helper4GetNextStupidTile(from.Left, to, ref nextPos, ref minDistance);
        helper4GetNextStupidTile(from.UpRight, to, ref nextPos, ref minDistance);
        helper4GetNextStupidTile(from.UpLeft, to, ref nextPos, ref minDistance);
        helper4GetNextStupidTile(from.DownLeft, to, ref nextPos, ref minDistance);
        helper4GetNextStupidTile(from.DownRight, to, ref nextPos, ref minDistance);

        if (nextPos.X == -1)
            return Position.NullPosition;

        return nextPos;
    }
    // helper ////////
    private static void helper4GetNextStupidTile(Position from, Position to, ref Position nextPos, ref int minDistance)
    {
        if (from.DistanceTo(to) < minDistance && !Dungeon.Instance.GetTile(from).IsBlockPath)
        {
            nextPos = from;
            minDistance = from.DistanceTo(to);
        }
    }
    // ====================================================================================== //
    // ====================================================================================== //
    // ====================================================================================== //
}
