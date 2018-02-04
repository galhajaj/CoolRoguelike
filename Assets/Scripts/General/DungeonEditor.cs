using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DungeonEditor : Singleton<DungeonEditor>
{
    private const string CURRENT_PLAYER = "Player1"; // temp...hard coded in the meantime. get it from text somehow
    private const string SAVE_FILE_PATH = @"c:/temp"; // change location and name to be for the current player

    [SerializeField]
    private Grid _grid = null;
    [SerializeField]
    private Text _textNumber = null;
    [SerializeField]
    private InputField _inputField = null;

    private DungeonSaveData _dungeonSaveData = new DungeonSaveData();

    private int _number = 1;

    void Start()
    {

    }

    void Update()
    {
        checkLeftClickOnTile();
        checkRightClickOnTile();
        checkPressNumber();
    }

    public void ClickSave()
    {
        /*string path = Application.dataPath + "/dungeons";

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);*/

        Utils.WriteToBinaryFile(SAVE_FILE_PATH + "/" + _inputField.text + ".dat", _dungeonSaveData);
    }

    public void ClickLoad()
    {
        clear();
    }

    private void checkRightClickOnTile()
    {
        if (!Input.GetMouseButtonDown(1))
            return;

        LayerMask layerMask = (1 << LayerMask.NameToLayer("DungeonTile"));
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.0F, layerMask);
        if (hit.collider == null)
            return;

        DungeonTile targetTile = hit.collider.GetComponent<DungeonTile>();
        if (targetTile == null)
            return;

        targetTile.Clear();
    }

    private void checkLeftClickOnTile()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        LayerMask layerMask = (1 << LayerMask.NameToLayer("DungeonTile"));
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.0F, layerMask);
        if (hit.collider == null)
            return;

        DungeonTile targetTile = hit.collider.GetComponent<DungeonTile>();
        if (targetTile == null)
            return;

        if (_number == 1)
        {
            targetTile.Clear();
        }
        else if (_number == 2)
        {
            putDungeonObject(targetTile.PosX, targetTile.PosY, ResourcesManager.Instance.WallPrefab);
        }
        else if (_number == 3)
        {
            putDungeonObject(targetTile.PosX, targetTile.PosY, ResourcesManager.Instance.SwordItemPrefab);
        }
        else if (_number == 4)
        {
            putDungeonObject(targetTile.PosX, targetTile.PosY, ResourcesManager.Instance.DragonPrefab);
        }
        else if (_number == 5)
        {
            putPortal(targetTile.PosX, targetTile.PosY, "Village");
        }
    }

    private void checkPressNumber()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // none
            _number = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) // wall
            _number = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) // dragon
            _number = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) // sword
            _number = 4;
        if (Input.GetKeyDown(KeyCode.Alpha5)) // sword
            _number = 5;

        _textNumber.text = "Number = " + _number.ToString();
    }

    private void clear()
    {
        foreach (DungeonTile tile in _grid.Elements)
            tile.Clear();
    }

    private DungeonObject putDungeonObject(int x, int y, GameObject prefab)
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
    }

    private void putPortal(int x, int y, string leadTo)
    {
        DungeonObject portalObj = putDungeonObject(x, y, ResourcesManager.Instance.StairsPrefab);

        Portal portal = portalObj.GetComponent<Portal>();
        portal.LeadTo = leadTo;
    }

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

    public DungeonTile GetTile(Position pos)
    {
        if (pos == Position.NullPosition)
            return null;
        return _grid.GetElement(pos) as DungeonTile;
    }
}
