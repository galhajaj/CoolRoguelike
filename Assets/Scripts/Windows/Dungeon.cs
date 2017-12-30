using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Dungeon : Singleton<Dungeon> 
{
    [SerializeField]
    private int _boardSizeX = 0;
    [SerializeField]
    private int _boardSizeY = 0;
    [SerializeField]
    private Transform _tilePrefab = null;
    [SerializeField]
    private Transform _wallPrefab = null;

    private List<List<DungeonTile>> _tiles = new List<List<DungeonTile>>();

    // ======================================================================================================================================== //
    protected override void AfterAwake()
    {
        generateTiles();
    }
    // ======================================================================================================================================== //
    void Start() 
	{

	}
    // ======================================================================================================================================== //
    public void Load(string dungeonName)
    {
        // un-parent the party to prevent deletion
        Party.Instance.transform.parent = null;

        // make all empty
        foreach (var tileList in _tiles)
        {
            foreach (var tile in tileList)
            {
                tile.Type = SurfaceType.EMPTY;
                // delete object inside tile
                if (tile.transform.childCount > 0)
                    Destroy(tile.transform.GetChild(0).gameObject);
            }
        }

        // TODO: implement taking from files or something else...
        if (dungeonName == "Ancient_Castle_Level_1")
        {
            putWall(3, 5);
            putWall(3, 6);
            putWall(3, 7);
            putWall(3, 8);

            putWall(4, 5);
            putWall(5, 5);
            putWall(6, 5);

            putStairs(12, 5, "Village");
            putStairs(15, 15, "Ancient_Castle_Level_2");
        }
        else if (dungeonName == "Ancient_Castle_Level_2")
        {
            putWall(9, 5);
            putWall(9, 6);
            putWall(9, 7);
            putWall(9, 8);

            putStairs(12, 12, "Ancient_Castle_Level_1");
            putStairs(15, 17, "Ancient_Castle_Level_3");
        }
        else if (dungeonName == "Ancient_Castle_Level_3")
        {
            putWall(20, 5);
            putWall(20, 6);
            putWall(20, 7);
            putWall(20, 8);

            putStairs(12, 19, "Ancient_Castle_Level_2");
        }
        else if (dungeonName == "Village")
        {
            // nothing
        }
        else
        {
            Debug.LogError("dungeonName: [" + dungeonName + "] not found!");
        }

        // find the stirs leads to location the party came from
        DungeonTile startStairs = null;
        foreach (var tileList in _tiles)
            foreach (var tile in tileList)
                if (tile.Type == SurfaceType.STAIRS)
                    if (tile.LeadTo == Party.Instance.Loaction)
                        startStairs = tile;
        if (startStairs == null && dungeonName != "Village")
        {
            Debug.LogError("cannot find stairs that lead to " + Party.Instance.Loaction);
        }
        // place the party
        if (dungeonName != "Village")
            Dungeon.Instance.PlaceObject(Party.Instance.gameObject, startStairs.Position);
        // update the location of the party
        Party.Instance.Loaction = dungeonName;
    }
    // ======================================================================================================================================== //
    public DungeonTile GetTile(Position pos)
    {
        return _tiles[pos.X][pos.Y];
    }
    // ======================================================================================================================================== //
    // good for party, creature, chests and another things that can be only one of them in tile and can be move from there 
    // or has a special interaction
    public void PlaceObject(GameObject obj, Position pos)
    {
        DungeonTile targetTile = GetTile(pos);

        if (targetTile.IsContainObject)
        {
            Debug.LogError(obj.name + " cannot be placed at (" + pos.X + "," + pos.Y + ")");
            return;
        }

        obj.transform.position = targetTile.transform.position;
        obj.transform.parent = targetTile.transform;
    }
    // ======================================================================================================================================== //
    private void putWall(int x, int y)
    {
        GameObject wall = Instantiate(_wallPrefab).gameObject;
        Position pos = new Position(x, y);
        PlaceObject(wall, pos);
    }
    // ======================================================================================================================================== //
    private void putStairs(int x, int y, string leadTo)
    {
        _tiles[x][y].SetAsStairs(leadTo);
    }
    // ======================================================================================================================================== //
    private void generateTiles()
	{
		float boardOriginX = this.transform.position.x;
		float boardOriginY = this.transform.position.y;
        float originOffsetX = -this.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float originOffsetY = this.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        float tileWidth = _tilePrefab.GetComponent<SpriteRenderer> ().bounds.size.x;
		float tileHeight = _tilePrefab.GetComponent<SpriteRenderer> ().bounds.size.y;
		
		for ( int x = 0; x < _boardSizeX; x++ ) 
		{
            _tiles.Add(new List<DungeonTile>());

            for ( int y = 0; y < _boardSizeY; y++ ) 
			{
				Vector3 tilePosition = new Vector3(
                    boardOriginX + originOffsetX + (tileWidth / 2) + (x * tileWidth), 
                    boardOriginY + originOffsetY - (tileHeight / 2) - (y * tileHeight), 
                    0);
				Transform tile = Instantiate(_tilePrefab, tilePosition, Quaternion.identity);
				tile.name = "Tile" + x + "_" + y;
				tile.parent = this.transform.Find("Tiles").transform;

                DungeonTile tileScript = tile.GetComponent<DungeonTile>();
                tileScript.PosX = x;
                tileScript.PosY = y;
                tileScript.Type = SurfaceType.EMPTY;

                _tiles[x].Add(tileScript);
            }
		}
	}
	// ======================================================================================================================================== //
}
