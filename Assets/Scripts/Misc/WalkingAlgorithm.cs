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
        Position nextPos = Position.NullPosition;
        int minDistance = 10000;

        helper4GetNextStupidTile(from.North, to, ref nextPos, ref minDistance);
        helper4GetNextStupidTile(from.South, to, ref nextPos, ref minDistance);
        helper4GetNextStupidTile(from.East, to, ref nextPos, ref minDistance);
        helper4GetNextStupidTile(from.West, to, ref nextPos, ref minDistance);
        helper4GetNextStupidTile(from.NorthEast, to, ref nextPos, ref minDistance);
        helper4GetNextStupidTile(from.NorthWest, to, ref nextPos, ref minDistance);
        helper4GetNextStupidTile(from.SouthWest, to, ref nextPos, ref minDistance);
        helper4GetNextStupidTile(from.SouthEast, to, ref nextPos, ref minDistance);

        return nextPos;
    }
    // helper ////////
    private static void helper4GetNextStupidTile(Position from, Position to, ref Position nextPos, ref int minDistance)
    {
        if (Dungeon.Instance.GetTile(from) == null)
            return;

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
