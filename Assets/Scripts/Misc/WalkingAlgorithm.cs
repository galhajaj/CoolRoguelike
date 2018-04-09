using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingAlgorithm
{
    // ====================================================================================== //
    public static Position GetNextPosition(Position origin, Position destination)
    {
        // if pointing on creature - use the my stupid path finding
        if (destination.DungeonTile.IsBlockPath)
            return getNextStupidTile(origin, destination);

        // else, using A* of 3rd party library from https://github.com/RonenNess/Unity-2d-pathfinding

        // create a grid
        PathFind.Grid grid = new PathFind.Grid(Dungeon.Instance.Width, Dungeon.Instance.Height, Dungeon.Instance.GetWalkingMap());

        // create source and target points
        PathFind.Point _from = new PathFind.Point(origin.X, origin.Y);
        PathFind.Point _to = new PathFind.Point(destination.X, destination.Y);

        // get path
        // path will either be a list of Points (x, y), or an empty list if no path is found.
        List<PathFind.Point> path = PathFind.Pathfinding.FindPath(grid, _from, _to);

        // if no path return null position
        if (path.Count <= 0)
            return Position.NullPosition;
        // otherwise, return the first position in path
        return new Position(path[0].x, path[0].y);
    }
    // ====================================================================================== //
    // stupid path finding
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
}
