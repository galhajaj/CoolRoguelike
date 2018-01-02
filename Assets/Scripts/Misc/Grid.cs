using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private int _boardSizeX = 0;
    [SerializeField]
    private int _boardSizeY = 0;

    [SerializeField]
    public Transform _elementPrefab = null;

    private List<List<GridTile>> _elements = new List<List<GridTile>>();

    public List<GridTile> Tiles { get { return getTileList(); } }

    void Awake()
    {
        
    }

    void Start ()
    {
        generateTiles();
    }
	
	void Update ()
    {
		
	}

    private void generateTiles()
    {
        float boardOriginX = this.transform.position.x;
        float boardOriginY = this.transform.position.y;
        float originOffsetX = -this.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float originOffsetY = this.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        float tileWidth = _elementPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float tileHeight = _elementPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        for (int x = 0; x < _boardSizeX; x++)
        {
            _elements.Add(new List<GridTile>());

            for (int y = 0; y < _boardSizeY; y++)
            {
                Vector3 tilePosition = new Vector3(
                    boardOriginX + originOffsetX + (tileWidth / 2) + (x * tileWidth),
                    boardOriginY + originOffsetY - (tileHeight / 2) - (y * tileHeight),
                    0);
                Transform tile = Instantiate(_elementPrefab, tilePosition, Quaternion.identity);
                tile.name = "Tile" + x + "_" + y;
                tile.parent = this.transform;

                GridTile tileScript = tile.GetComponent<GridTile>();
                tileScript.PosX = x;
                tileScript.PosY = y;

                _elements[x].Add(tileScript);
            }
        }
    }

    private List<GridTile> getTileList()
    {
        List<GridTile> result = new List<GridTile>();

        foreach (var tileList in _elements)
            foreach (var tile in tileList)
                result.Add(tile);

        return result;
    }

    public GridTile GetTile(int x, int y = 0)
    {
        return _elements[x][y];
    }

    public GridTile GetTile(Position pos)
    {
        return GetTile(pos.X, pos.Y);
    }
}
