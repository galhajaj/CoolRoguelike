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


            putPortal(12, 5, "Village");
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
}
