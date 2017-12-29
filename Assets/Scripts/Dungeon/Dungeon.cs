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
        // make all empty
        foreach (var tileList in _tiles)
            foreach (var tile in tileList)
                tile.Type = SurfaceType.EMPTY;

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

            _tiles[12][5].Type = SurfaceType.STAIRS;
            _tiles[15][15].Type = SurfaceType.STAIRS;
        }
        else if (dungeonName == "Ancient_Castle_Level_1")
        {
            _tiles[3][5].Type = SurfaceType.WALL;
            //_tiles[3][6].Type = SurfaceType.WALL;
            _tiles[3][7].Type = SurfaceType.WALL;
            _tiles[3][8].Type = SurfaceType.WALL;

            _tiles[4][5].Type = SurfaceType.WALL;
            //_tiles[5][5].Type = SurfaceType.WALL;
            _tiles[6][5].Type = SurfaceType.WALL;
            _tiles[7][5].Type = SurfaceType.WALL;

            _tiles[12][12].Type = SurfaceType.STAIRS;
            _tiles[15][15].Type = SurfaceType.STAIRS;
        }
        else if (dungeonName == "Ancient_Castle_Level_1")
        {
            _tiles[3][5].Type = SurfaceType.WALL;
            _tiles[3][6].Type = SurfaceType.WALL;
            //_tiles[3][7].Type = SurfaceType.WALL;
            _tiles[3][8].Type = SurfaceType.WALL;

            _tiles[4][5].Type = SurfaceType.WALL;
            _tiles[5][5].Type = SurfaceType.WALL;
            //_tiles[6][5].Type = SurfaceType.WALL;
            _tiles[7][5].Type = SurfaceType.WALL;

            _tiles[12][12].Type = SurfaceType.STAIRS;
        }
        else
        {
            Debug.LogError("dungeonName: [" + dungeonName + "] not found!");
        }
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
