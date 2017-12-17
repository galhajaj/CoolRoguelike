using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Dungeon : MonoBehaviour 
{
	public int BoardSizeX;
	public int BoardSizeY;
	public Transform TilePrefab;

    public Sprite WallSprite;

    //private List<Transform> _tiles = new List<Transform>();
    private List<List<Transform>> _tiles = new List<List<Transform>>();

    // ======================================================================================================================================== //
    void Awake()
    {
        generateTiles();
        buildDungeon();
    }
    // ======================================================================================================================================== //
    void Start() 
	{

	}
    // ======================================================================================================================================== //
    public void Load(string dungeonName)
    {
        // imp.
    }
    // ======================================================================================================================================== //
    private void generateTiles()
	{
		float boardOriginX = this.transform.position.x;
		float boardOriginY = this.transform.position.y;
		float tileWidth = TilePrefab.GetComponent<SpriteRenderer> ().bounds.size.x;
		float tileHeight = TilePrefab.GetComponent<SpriteRenderer> ().bounds.size.y;
		
		for ( int x = 0; x < BoardSizeX; x++ ) 
		{
            _tiles.Add(new List<Transform>());

            for ( int y = 0; y < BoardSizeY; y++ ) 
			{
				Vector3 tilePosition = new Vector3(boardOriginX + (tileWidth / 2) + (x * tileWidth), boardOriginY + (tileHeight / 2) + (y * tileHeight), 0);
				Transform tile = (Transform)Instantiate(TilePrefab, tilePosition, Quaternion.identity);
				tile.name = "Tile" + x + "_" + y;
				tile.parent = this.transform;

				tile.GetComponent<Tile>().PosX = x;
				tile.GetComponent<Tile>().PosY = y;

                _tiles[x].Add(tile);
                //_tiles.Add(tile);
            }
		}
	}
	// ======================================================================================================================================== //
    private void buildDungeon()
    {
        _tiles[3][5].GetComponent<SpriteRenderer>().sprite = WallSprite;
        _tiles[3][6].GetComponent<SpriteRenderer>().sprite = WallSprite;
        _tiles[3][7].GetComponent<SpriteRenderer>().sprite = WallSprite;
        _tiles[3][8].GetComponent<SpriteRenderer>().sprite = WallSprite;

        _tiles[4][5].GetComponent<SpriteRenderer>().sprite = WallSprite;
        _tiles[5][5].GetComponent<SpriteRenderer>().sprite = WallSprite;
        _tiles[6][5].GetComponent<SpriteRenderer>().sprite = WallSprite;
        _tiles[7][5].GetComponent<SpriteRenderer>().sprite = WallSprite;
    }
    // ======================================================================================================================================== //
}
