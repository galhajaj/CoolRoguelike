using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightAlgorithm
{
    const int SIGHT_RADIUS = 6;
    const int ANGLE_STEP = 5;

    // TODO: improve the performance of the sight algorithm if needed (ANGLE_STEP is a candidate) (or bake it before)
    // ====================================================================================== //
    public static bool CanSee(Position source, Position target)
    {
        // can't see if too far
        if (source.DistanceTo(target) > SIGHT_RADIUS)
            return false;


        // check if on the line there is a view blocker
        foreach (Position pointInLine in line(source.X, source.Y, target.X, target.Y))
        {
            if (pointInLine.DungeonTile == null)
                return false;
            if (pointInLine.DungeonTile.IsBlockView)
                return false;
        }

        return true;
    }
    // ====================================================================================== //
    public static List<Position> GetPositionsInSight(Position center)
    {
        List<Position> points = new List<Position>();

        // move over all points on circle
        foreach (Position pointOnCircle in pointsOnCircle(center, SIGHT_RADIUS, ANGLE_STEP))
        {
            // check the line between the center to each point on circle
            // get all positions until it stuck with view blocker (include the blocker)
            foreach (Position pointInLine in line(center.X, center.Y, pointOnCircle.X, pointOnCircle.Y))
            {
                if (pointInLine.DungeonTile == null)
                    break;

                if (!pointInLine.DungeonTile.IsBlockView)
                {
                    if (!points.Contains(pointInLine))
                        points.Add(pointInLine);
                }
                else
                {
                    points.Add(pointInLine);
                    break;
                }
            }
        }

        return points;
    }
    // ====================================================================================== //
    // Bresenham's line algorithm
    private static List<Position> line(int x, int y, int x2, int y2)
    {
        List<Position> points = new List<Position>();

        int w = x2 - x;
        int h = y2 - y;
        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
        if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
        if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
        if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
        int longest = Math.Abs(w);
        int shortest = Math.Abs(h);
        if (!(longest > shortest))
        {
            longest = Math.Abs(h);
            shortest = Math.Abs(w);
            if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
            dx2 = 0;
        }
        int numerator = longest >> 1;
        for (int i = 0; i <= longest; i++)
        {
            points.Add(new Position(x, y));
            numerator += shortest;
            if (!(numerator < longest))
            {
                numerator -= longest;
                x += dx1;
                y += dy1;
            }
            else
            {
                x += dx2;
                y += dy2;
            }
        }

        return points;
    }
    // ====================================================================================== //
    private static List<Position> pointsOnCircle(Position center, int radius, int angleStep)
    {
        List<Position> points = new List<Position>();

        for (float angle = 0; angle < 360; angle += angleStep)
        {
            int X = center.X + (int)(radius * Math.Cos(Mathf.Deg2Rad * angle));
            int Y = center.Y + (int)(radius * Math.Sin(Mathf.Deg2Rad * angle));
            Position newPoint = new Position(X, Y);

            if (!points.Contains(newPoint))
                points.Add(newPoint);
        }

        return points;
    }
    // ====================================================================================== //
}
