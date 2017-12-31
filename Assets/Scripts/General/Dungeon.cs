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
    public Transform _tilePrefab = null;

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
            Dungeon.Instance.SetDungeonObjectPosition(Party.Instance.DungeonObject, startStairs.Position);
        // update the location of the party
        Party.Instance.Loaction = dungeonName;
    }
    // ======================================================================================================================================== //
    private DungeonTile findPortalTile(string leadTo)
    {
        foreach (var tileList in _tiles)
        {
            foreach (var tile in tileList)
            {
                Portal portal = tile.GetComponentInChildren<Portal>();
                if (portal != null)
                {
                    if (portal.LeadTo == leadTo)
                        return tile;
                }
            }
        }

        return null;
    }
    // ======================================================================================================================================== //
    public DungeonTile GetTile(Position pos)
    {
        return _tiles[pos.X][pos.Y];
    }
    // ======================================================================================================================================== //
    // good for party, creature, chests and another things that can be only one of them in tile and can be move from there 
    // or has a special interaction
    public void SetDungeonObjectPosition(DungeonObject obj, Position pos)
    {
        DungeonTile targetTile = GetTile(pos);

        if (targetTile.IsBlockPath)
        {
            Debug.LogError(obj.gameObject.name + " cannot be placed at (" + pos.X + "," + pos.Y + ")");
            return;
        }

        obj.transform.position = targetTile.transform.position;
        obj.transform.parent = targetTile.transform;
    }
    // ======================================================================================================================================== //
    private DungeonObject putDungeonObject(int x, int y, GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        DungeonObject dungeonObject = obj.GetComponent<DungeonObject>();
        Position pos = new Position(x, y);
        SetDungeonObjectPosition(dungeonObject, pos);
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
        // clear each tile
        foreach (var tileList in _tiles)
            foreach (var tile in tileList)
                tile.Clear();
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

                _tiles[x].Add(tileScript);
            }
		}
	}
	// ======================================================================================================================================== //
}
