﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Dungeon : Singleton<Dungeon> 
{
    [SerializeField]
    private Grid _grid = null;

    private DungeonSaveData _dungeonSaveData = null;
    private Position _currentShownAreaPosition;

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

        // find matching dungeon save data from player data by its name
        
        foreach (DungeonSaveData data in SaveAndLoad.Instance.PlayerSaveData.Dungeons)
            if (data.Name == dungeonName)
                _dungeonSaveData = data;

        // return if not exist
        if (_dungeonSaveData == null)
        {
            Debug.LogError(dungeonName + " dungeon doesn't exist in player save data");
            return;
        }

        // TODO: in the meantime... show always first...
        showArea(new Position(0, 0, 0));

        // TODO: implement taking from files or something else...
        // ###############################################################
        /*if (dungeonName == "Ancient_Castle_Level_1")
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
        }*/
        // ###############################################################

        // find the stairs that lead to the location that the party came from it
        DungeonTile startStairs = findPortalTile(Party.Instance.Loaction);

        if (startStairs == null && dungeonName != "Village")
        {
            Debug.LogError("cannot find stairs that lead to " + Party.Instance.Loaction);
        }
        // place the party
        if (dungeonName != "Village")
        {
            DungeonTile tile = GetTile(new Position(0, 0)); // TODO: change that
            PutDungeonObjectInTile(Party.Instance.DungeonObject, tile);
        }
        // update the location of the party
        Party.Instance.Loaction = dungeonName;
    }

    // $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
    private void showArea(Position position) // return the area index
    {
        // clear current grid
        clear();

        // get the area
        AreaSaveData areaToShow = getArea(position);

        // put area stuff in grid.TODO: why only from stuff... it's a mistake, it contains also the creatures etc.
        foreach (StuffSaveData stuff in areaToShow.Stuff)
        {
            DungeonTile tile = _grid.GetElement(stuff.Position) as DungeonTile;
            putObjectInTile(stuff.Name, tile);
        }

        // update current area index
        _currentShownAreaPosition = position;
    }

    private void putObjectInTile(string objectName, DungeonTile targetTile)
    {
        // get object from resources folder
        var allResources = Resources.LoadAll<GameObject>("");
        GameObject prefabToCreate = null;
        foreach (GameObject obj in allResources)
            if (obj.name == objectName)
                prefabToCreate = obj;

        if (prefabToCreate == null)
        {
            Debug.LogError(objectName + " resource doesn't exist");
            return;
        }

        // instantiate it in tile
        GameObject instance = Instantiate(prefabToCreate);
        instance.transform.position = targetTile.transform.position;
        instance.transform.parent = targetTile.transform;
    }

    private AreaSaveData getArea(Position position)
    {
        if (!_dungeonSaveData.Areas.ContainsKey(position))
        {
            AreaSaveData newArea = new AreaSaveData();
            newArea.Position = position;
            _dungeonSaveData.Areas[position] = newArea;
        }

        return _dungeonSaveData.Areas[position];
    }
    // $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

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
        if (pos == Position.NullPosition)
            return null;
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
    /*private DungeonObject putDungeonObject(int x, int y, GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        DungeonObject dungeonObject = obj.GetComponent<DungeonObject>();
        Position pos = new Position(x, y);
        DungeonTile targetTile = GetTile(pos);
        PutDungeonObjectInTile(dungeonObject, targetTile);

        // if item, set sprite to loot icon
        Item item = obj.GetComponent<Item>();
        if (item != null)
            item.State = ItemState.GROUND;

        return dungeonObject;
    }*/
    // ======================================================================================================================================== //
    /*private void putPortal(int x, int y, string leadTo)
    {
        DungeonObject portalObj = putDungeonObject(x, y, ResourcesManager.Instance.StairsPrefab);

        Portal portal = portalObj.GetComponent<Portal>();
        portal.LeadTo = leadTo;
    }*/
    // ======================================================================================================================================== //
    private void clear()
    {
        foreach (DungeonTile tile in _grid.Elements)
            tile.Clear();
    }
    // ======================================================================================================================================== //
    public List<List<bool>> GetWalkingMap()
    {
        List<List<bool>> walkingMap = new List<List<bool>>();

        for (int x = 0; x < _grid.SizeX; x++)
        {
            walkingMap.Add(new List<bool>());

            for (int y = 0; y < _grid.SizeY; y++)
            {
                DungeonTile tile = GetTile(new Position(x, y));
                walkingMap[x].Add(!tile.IsBlockPath);
            }
        }

        return walkingMap;
    }
    // ======================================================================================================================================== //
    public List<List<bool>> GetSightMap()
    {
        List<List<bool>> sightMap = new List<List<bool>>();

        for (int x = 0; x < _grid.SizeX; x++)
        {
            sightMap.Add(new List<bool>());

            for (int y = 0; y < _grid.SizeY; y++)
            {
                DungeonTile tile = GetTile(new Position(x, y));
                sightMap[x].Add(!tile.IsBlockView);
            }
        }

        return sightMap;
    }
    // ======================================================================================================================================== //
}
