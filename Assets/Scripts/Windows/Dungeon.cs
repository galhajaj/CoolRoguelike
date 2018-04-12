using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Dungeon : Singleton<Dungeon> 
{
    [SerializeField]
    private GenericGrid _grid = null;

    private DungeonSaveData _dungeonSaveData = null;
    private Position _currentShownAreaPosition;

    public bool IsInOriginArea { get { return _currentShownAreaPosition == Position.OriginPosition; } }

    public int Width { get { return _grid.SizeX; } }
    public int Height { get { return _grid.SizeY; } }

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
        if (dungeonName != Consts.WindowNames.VILLAGE)
        {
            DungeonTile tile = GetTile(new Position(0, 0)); // TODO: change that
            PutDungeonObjectInTile(Party.Instance.DungeonObject, tile);
            RevealPartySurroundings();
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
            GameObject createdObj = createObjectInTile(objSaveData.Name, tile);

            // add crried items on creature
            if (objSaveData is CreatureSaveData)
            {
                CreatureSaveData creatureSaveData = objSaveData as CreatureSaveData;
                Creature creature = createdObj.GetComponent<Creature>();
                foreach (ItemSaveData itemSaveData in creatureSaveData.CarriedItems)
                {
                    createObjectInCreature(itemSaveData.Name, creature);
                }
            }
        }

        // fog of war
        foreach (DungeonTile tile in _grid.Elements)
            tile.IsRevealed = areaToShow.TileRevealationMap[tile.Position.X, tile.Position.Y];

        // update current area index
        _currentShownAreaPosition = position;

        // update the dungeon tile sprite - grass or floor
        foreach (DungeonTile tile in _grid.Elements)
        {
            if (IsInOriginArea)
                tile.GetComponent<SpriteRenderer>().sprite = ResourcesManager.Instance.OutdoorDungeonTileSprite;
            else
                tile.GetComponent<SpriteRenderer>().sprite = ResourcesManager.Instance.FloorDungeonTileSprite;
        }
    }
    // ================================================================================================== //
    public void ShowArea(Direction direction)
    {
        Position areaPosition = _currentShownAreaPosition.GetPositionInDirection(direction);
        ShowArea(areaPosition);
    }
    // ================================================================================================== //
    private GameObject createObjectInTile(string objectName, DungeonTile targetTile)
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
            return null;
        }

        // instantiate it in tile
        GameObject instance = Instantiate(prefabToCreate);
        instance.transform.position = targetTile.transform.position;
        instance.transform.parent = targetTile.transform;
        // if item, make it in ground state, so the sprite will change to icon
        if (instance.GetComponent<Item>() != null)
            instance.GetComponent<Item>().State = ItemState.GROUND;

        return instance;
    }
    // ================================================================================================== //
    private void createObjectInCreature(string objectName, Creature creature)
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
        instance.transform.position = creature.transform.position;
        instance.transform.parent = creature.transform;

        Item item = instance.GetComponent<Item>();

        // add to creature carried items
        creature.CarriedItems.Add(item);

        // its a carried item - hide it and change its state to carried
        item.Hide();
        item.State = ItemState.EQUIPPED;

        // try to put it on
        creature.EquipItem(item);
    }
    // ================================================================================================== //
    public DungeonTile GetTile(Position pos)
    {
        if (pos == Position.NullPosition)
            return null;

        if (pos.X < 0 || pos.X >= Width || pos.Y < 0 || pos.Y >= Height)
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
    public bool[,] GetWalkingMap()
    {
        bool[,] walkingMap = new bool[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                DungeonTile tile = GetTile(new Position(x, y));
                walkingMap[x, y] = !tile.IsBlockPath;
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
    public void SaveCurrentArea()
    {
        AreaSaveData areaSaveData = new AreaSaveData();

        areaSaveData.Position = _currentShownAreaPosition;

        areaSaveData.TileRevealationMap = new bool[Width, Height];

        // add objects to area data
        foreach (DungeonTile tile in _grid.Elements)
        {
            // update tile revealation map
            areaSaveData.TileRevealationMap[tile.Position.X, tile.Position.Y] = tile.IsRevealed;

            foreach (DungeonObject dungeonObj in tile.transform.GetComponentsInChildren<DungeonObject>())
            {
                if (dungeonObj is Creature)
                    if ((dungeonObj as Creature).IsPartyMember)
                        continue;

                areaSaveData.Objects.Add(dungeonObj.GetSaveData());
            }
        }

        // update current area in _dungeonSaveData
        _dungeonSaveData.Areas[_currentShownAreaPosition] = areaSaveData;
        // save to file
        SaveAndLoad.Instance.Save();
    }
    // ================================================================================================== //
    public void RevealPartySurroundings()
    {
        foreach (Position pos in SightAlgorithm.GetPositionsInSight(Party.Instance.Position))
            if (pos.DungeonTile != null)
                pos.DungeonTile.IsRevealed = true;
    }
    // ================================================================================================== //
}
