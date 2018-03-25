using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScroll : Scroll
{
    protected override bool activate()
    {
        // if bloack - can't go there
        if (TargetDungeonTile.IsBlockPath)
            return false;

        Dungeon.Instance.PutDungeonObjectInTile(Party.Instance.DungeonObject, TargetDungeonTile);

        return true;
    }
}
