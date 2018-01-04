using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Dungeon : Singleton<Dungeon> 
{
    [SerializeField]
    private Grid _grid = null;

    // ======================================================================================================================================== //
    void Start() 
	{

	}
    // ======================================================================================================================================== //
    public void Load(string dungeonName)
    {
        // un-parent the party to prevent deletion
        Party.Instance.transform.parent = null;

        // clean all dungeon
        this.clear();

        // TODO: implement taking from files or something else...
        // ###############################################################
        if (dungeonName == "Ancient_Castle_Level_1")
        {
            putDungeonObject(3, 5, ResourcesManager.Instance.WallPrefab);
            putDungeonObject(3, 6, ResourcesManager.Instance.WallPrefab);
            putDungeonObject(3, 7, ResourcesManager.Instance.WallPrefab);
            putDungeonObject(3, 8, ResourcesManager.Instance.WallPrefab);

            putDungeonObject(4, 5, ResourcesManager.Instance.WallPrefab);
            putDungeonObject(5, 5, ResourcesManager.Instance.WallPrefab);
            putDungeonObject(6, 5, ResourcesManager.Instance.WallPrefab);

            putDungeonObject(8, 8, ResourcesManager.Instance.SwordItemPrefab);

            putDungeonObject(10, 10, ResourcesManager.Instance.DragonPrefab);
            putDungeonObject(13, 13, ResourcesManager.Instance.DragonPrefab);


            putPortal(0, 0, "Village");
            putPortal(15, 15, "Ancient_Castle_Level_2");
        }
        else if (dungeonName == "Ancient_Castle_Level_2")
        {
            putDungeonObject(9, 5, ResourcesManager.Instance.WallPrefab);
            putDungeonObject(9, 6, ResourcesManager.Instance.WallPrefab);
            putDungeonObject(9, 7, ResourcesManager.Instance.WallPrefab);
            putDungeonObject(9, 8, ResourcesManager.Instance.WallPrefab);

            putPortal(12, 12, "Ancient_Castle_Level_1");
            putPortal(15, 17, "Ancient_Castle_Level_3");
        }
        else if (dungeonName == "Ancient_Castle_Level_3")
        {
            putDungeonObject(20, 5, ResourcesManager.Instance.WallPrefab);
            putDungeonObject(20, 6, ResourcesManager.Instance.WallPrefab);
            putDungeonObject(20, 7, ResourcesManager.Instance.WallPrefab);
            putDungeonObject(20, 8, ResourcesManager.Instance.WallPrefab);

            putPortal(12, 19, "Ancient_Castle_Level_2");
        }
        else if (dungeonName == "Village")
        {
            // nothing
        }
        else
        {
            Debug.LogError("dungeonName: [" + dungeonName + "] not found!");
        }
        // ###############################################################

        // find the stairs that lead to the location that the party came from it
        DungeonTile startStairs = findPortalTile(Party.Instance.Loaction);

        if (startStairs == null && dungeonName != "Village")
        {
            Debug.LogError("cannot find stairs that lead to " + Party.Instance.Loaction);
        }
        // place the party
        if (dungeonName != "Village")
            Dungeon.Instance.PutDungeonObjectInTile(Party.Instance.DungeonObject, startStairs);
        // update the location of the party
        Party.Instance.Loaction = dungeonName;
    }
    // ======================================================================================================================================== //
    private DungeonTile findPortalTile(string leadTo)
    {
        foreach (DungeonTile tile in _grid.Elements)
        {
            Portal portal = tile.GetComponentInChildren<Portal>();
            if (portal != null)
            {
                if (portal.LeadTo == leadTo)
                    return tile;
            }
        }

        return null;
    }
    // ======================================================================================================================================== //
    public DungeonTile GetTile(Position pos)
    {
        return _grid.GetElement(pos) as DungeonTile;
    }
    // ======================================================================================================================================== //
    public List<Creature> GetCreatures()
    {
        List<Creature> creatures = new List<Creature>();

        foreach (DungeonTile tile in _grid.Elements)
        {
            Creature creature = tile.GetContainedCreature();
            if (creature != null)
                creatures.Add(creature);
        }

        return creatures;
    }
    // ======================================================================================================================================== //
    // good for party, creature, chests and another things that can be only one of them in tile and can be move from there 
    // or has a special interaction
    public void PutDungeonObjectInTile(DungeonObject obj, DungeonTile tile)
    {
        if (tile.IsBlockPath)
        {
            Debug.LogError(obj.gameObject.name + " cannot be placed at (" + tile.PosX + "," + tile.PosY + ")");
            return;
        }

        obj.transform.position = tile.transform.position;
        obj.transform.parent = tile.transform;
    }
    // ======================================================================================================================================== //
    private DungeonObject putDungeonObject(int x, int y, GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        DungeonObject dungeonObject = obj.GetComponent<DungeonObject>();
        Position pos = new Position(x, y);
        DungeonTile targetTile = GetTile(pos);
        PutDungeonObjectInTile(dungeonObject, targetTile);

        // if item, set sprite to loot icon
        Item item = obj.GetComponent<Item>();
        if (item != null)
            item.SetSpriteToDungeonIcon();

        return dungeonObject;
    }
    // ======================================================================================================================================== //
    private void putPortal(int x, int y, string leadTo)
    {
        DungeonObject portalObj = putDungeonObject(x, y, ResourcesManager.Instance.StairsPrefab);

        Portal portal = portalObj.GetComponent<Portal>();
        portal.LeadTo = leadTo;
    }
    // ======================================================================================================================================== //
    private void clear()
    {
        foreach (DungeonTile tile in _grid.Elements)
            tile.Clear();
    }
    // ======================================================================================================================================== //
    // stupid path - to be deleted! :-)
    public DungeonTile GetNextStupidTile(Position from, Position to)
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
        /*if (from.Up.DistanceTo(to) < minDistance && !GetTile(from.Up).IsBlockPath)
        {
            nextPos = from.Up;
            minDistance = from.Up.DistanceTo(to);
        }
        if (from.Down.DistanceTo(to) < minDistance && !GetTile(from.Down).IsBlockPath)
        {
            nextPos = from.Down;
            minDistance = from.Down.DistanceTo(to);
        }
        if (from.Right.DistanceTo(to) < minDistance && !GetTile(from.Right).IsBlockPath)
        {
            nextPos = from.Right;
            minDistance = from.Right.DistanceTo(to);
        }
        if (from.Left.DistanceTo(to) < minDistance && !GetTile(from.Left).IsBlockPath)
        {
            nextPos = from.Left;
            minDistance = from.Left.DistanceTo(to);
        }

        if (from.UpRight.DistanceTo(to) < minDistance && !GetTile(from.UpRight).IsBlockPath)
        {
            nextPos = from.UpRight;
            minDistance = from.UpRight.DistanceTo(to);
        }
        if (from.DownRight.DistanceTo(to) < minDistance && !GetTile(from.DownRight).IsBlockPath)
        {
            nextPos = from.DownRight;
            minDistance = from.DownRight.DistanceTo(to);
        }
        if (from.UpLeft.DistanceTo(to) < minDistance && !GetTile(from.UpLeft).IsBlockPath)
        {
            nextPos = from.UpLeft;
            minDistance = from.UpLeft.DistanceTo(to);
        }
        if (from.DownLeft.DistanceTo(to) < minDistance && !GetTile(from.DownLeft).IsBlockPath)
        {
            nextPos = from.DownLeft;
            minDistance = from.DownLeft.DistanceTo(to);
        }*/

        if (nextPos.X == -1)
            return null;

        return this.GetTile(nextPos);
    }
    // helper ////////
    private void helper4GetNextStupidTile(Position from, Position to, ref Position nextPos, ref int minDistance)
    {
        if (from.DistanceTo(to) < minDistance && !GetTile(from).IsBlockPath)
        {
            nextPos = from;
            minDistance = from.DistanceTo(to);
        }
    }
    // ======================================================================================================================================== //
}
