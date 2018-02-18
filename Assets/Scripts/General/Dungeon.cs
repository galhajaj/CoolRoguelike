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
        ShowArea(Position.OriginPosition);

        // place the party
        if (dungeonName != Consts.VILLAGE)
        {
            DungeonTile tile = GetTile(new Position(0, 0)); // TODO: change that
            PutDungeonObjectInTile(Party.Instance.DungeonObject, tile);
        }
        // update the location of the party
        Party.Instance.Loaction = dungeonName;
    }
    // ================================================================================================== //
    private void ShowArea(Position position)
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

        // put area objects in grid.
        foreach (SaveData objSaveData in areaToShow.Objects)
        {
            DungeonTile tile = _grid.GetElement(objSaveData.Position) as DungeonTile;
            putObjectInTile(objSaveData.Name, tile);
        }

        // update current area index
        _currentShownAreaPosition = position;
    }
    // ================================================================================================== //
    public void ShowArea(Direction direction)
    {
        Position areaPosition = _currentShownAreaPosition.GetPositionInDirection(direction);
        ShowArea(areaPosition);
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
        // if item, make it in ground state, so the sprite will change to icon
        if (instance.GetComponent<Item>() != null)
            instance.GetComponent<Item>().State = ItemState.GROUND;
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
        // if item, make it in ground state, so the sprite will change to icon
        if (obj.GetComponent<Item>() != null)
            obj.GetComponent<Item>().State = ItemState.GROUND;
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
