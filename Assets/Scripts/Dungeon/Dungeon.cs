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
    private Transform _surfaceTilePrefab = null;
    [SerializeField]
    private Transform _partyMiniatureTransform = null;

    //private List<Transform> _tiles = new List<Transform>();
    private List<List<Surface>> _tiles = new List<List<Surface>>();

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
        foreach (var surfaceList in _tiles)
            foreach (var surface in surfaceList)
                surface.Type = SurfaceType.EMPTY;

        // TODO: implement taking from files or something else...
        if (dungeonName == "Ancient_Castle_Level_1")
        {
            _tiles[3][5].Type = SurfaceType.WALL;
            _tiles[3][6].Type = SurfaceType.WALL;
            _tiles[3][7].Type = SurfaceType.WALL;
            _tiles[3][8].Type = SurfaceType.WALL;

            _tiles[4][5].Type = SurfaceType.WALL;
            _tiles[5][5].Type = SurfaceType.WALL;
            _tiles[6][5].Type = SurfaceType.WALL;
            _tiles[7][5].Type = SurfaceType.WALL;

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
    public void SetPartyMiniaturePositionInDungeon()
    {
        Surface targetTile = _tiles[Party.Instance.Position.X][Party.Instance.Position.Y];
        _partyMiniatureTransform.position = targetTile.transform.position;
    }
    // ======================================================================================================================================== //
    private void generateTiles()
	{
		float boardOriginX = this.transform.position.x;
		float boardOriginY = this.transform.position.y;
        float originOffsetX = -this.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float originOffsetY = this.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        float tileWidth = _surfaceTilePrefab.GetComponent<SpriteRenderer> ().bounds.size.x;
		float tileHeight = _surfaceTilePrefab.GetComponent<SpriteRenderer> ().bounds.size.y;
		
		for ( int x = 0; x < _boardSizeX; x++ ) 
		{
            _tiles.Add(new List<Surface>());

            for ( int y = 0; y < _boardSizeY; y++ ) 
			{
				Vector3 tilePosition = new Vector3(
                    boardOriginX + originOffsetX + (tileWidth / 2) + (x * tileWidth), 
                    boardOriginY + originOffsetY - (tileHeight / 2) - (y * tileHeight), 
                    0);
				Transform tile = Instantiate(_surfaceTilePrefab, tilePosition, Quaternion.identity);
				tile.name = "Tile" + x + "_" + y;
				tile.parent = this.transform.Find("SurfaceTiles").transform;

                Surface surfaceScript = tile.GetComponent<Surface>();
                surfaceScript.PosX = x;
                surfaceScript.PosY = y;
                surfaceScript.Type = SurfaceType.EMPTY;

                _tiles[x].Add(surfaceScript);
            }
		}
	}
	// ======================================================================================================================================== //
}
