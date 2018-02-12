using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Dungeon : Singleton<Dungeon> 
{
    [SerializeField]
    private Grid _grid = null;

    private DungeonSaveData _dungeonSaveData = null;
    private Position _currentShownAreaPosition;

    public bool IsInOriginArea { get { return _currentShownAreaPosition == new Position(0, 0, 0); } }

    // ================================================================================================== //
    void Start() 
	{

	}
    // ================================================================================================== //
    public void Load(string dungeonName)
    {
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
        ShowArea(new Position(0, 0, 0));

        // place the party
        if (dungeonName != "Village")
        {
            DungeonTile tile = GetTile(new Position(0, 0)); // TODO: change that
            PutDungeonObjectInTile(Party.Instance.DungeonObject, tile);
        }
        // update the location of the party
        Party.Instance.Loaction = dungeonName;
    }
    // ================================================================================================== //
    public void ShowArea(Position position)
    {
        // check valid area
        if (!_dungeonSaveData.Areas.ContainsKey(position))
        {
            Debug.LogError("area doesn't exist in that position: " + position.ToString());
            return;
        }

        // clear current grid
        clear();

        // get the area
        AreaSaveData areaToShow = _dungeonSaveData.Areas[position];

        // put area stuff in grid.
        // TODO: why only from stuff... it's a mistake, it contains also the creatures etc.
        foreach (StuffSaveData stuff in areaToShow.Stuff)
        {
            DungeonTile tile = _grid.GetElement(stuff.Position) as DungeonTile;
            putObjectInTile(stuff.Name, tile);
        }

        // update current area index
        _currentShownAreaPosition = position;
    }
    // ================================================================================================== //
    public void ShowArea(Direction direction)
    {
        if (direction == Direction.NORTH)
            ShowArea(_currentShownAreaPosition.North);
        else if (direction == Direction.SOUTH)
            ShowArea(_currentShownAreaPosition.South);
        else if (direction == Direction.WEST)
            ShowArea(_currentShownAreaPosition.West);
        else if (direction == Direction.EAST)
            ShowArea(_currentShownAreaPosition.East);
        else if (direction == Direction.UP)
            ShowArea(_currentShownAreaPosition.Up);
        else if (direction == Direction.DOWN)
            ShowArea(_currentShownAreaPosition.Down);
        else
            Debug.LogError("area doesn't exist in that direction: " + direction.ToString());
    }
    // ================================================================================================== //
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
    // ================================================================================================== //
    public DungeonTile GetTile(Position pos)
    {
        if (pos == Position.NullPosition)
            return null;
        return _grid.GetElement(pos) as DungeonTile;
    }
    // ================================================================================================== //
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
    // ================================================================================================== //
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
    // ================================================================================================== //
    private void clear()
    {
        foreach (DungeonTile tile in _grid.Elements)
            tile.Clear();
    }
    // ================================================================================================== //
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
    // ================================================================================================== //
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
    // ================================================================================================== //
}
