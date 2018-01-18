using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightAlgorithm
{
    public static bool CanSee(Position source, Position target)
    {
        // TODO: implement
        // to delete, stupid implemnentation
        return stupidCanSee(source, target);
    }

    public static List<Position> GetSight(Position source)
    {
        // TODO: implement
        return null;
    }

    // ====================================================================================== //
    // stupid implementation... to be deleted...
    // ====================================================================================== //
    private static bool stupidCanSee(Position source, Position target)
    {
        int range = 6;
        if (source.DistanceTo(target) <= range)
            return true;
        return false;
    }
    // ====================================================================================== //
}
