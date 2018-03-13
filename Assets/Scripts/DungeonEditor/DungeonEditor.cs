using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DungeonEditor : Singleton<DungeonEditor>
{
    [SerializeField]
    private GenericGrid _grid = null;
    [SerializeField]
    private InputField _inputField = null;

    private DungeonSaveData _dungeonSaveData = new DungeonSaveData();
    private Position _currentShownAreaPosition;

    // ================================================================================================== //
    void Start()
    {
        showArea(Position.OriginPosition);
    }
    // ================================================================================================== //
    void Update()
    {
        checkLeftClickOnTile();
        checkRightClickOnTile();
    }
    // ================================================================================================== //
    private AreaSaveData getArea(Position position)
    {
        if (!_dungeonSaveData.Areas.ContainsKey(position))
        {
            AreaSaveData newArea = new AreaSaveData();
            newArea.Position = position;
            _dungeonSaveData.Areas[position] = newArea;
        }

        return _dungeonSaveData.Areas[position];
    }
    // ================================================================================================== //
    private void showArea(Position position) // return the area index
    {
        // clear current grid
        clear();

        // get the area
        AreaSaveData areaToShow = getArea(position);

        // putObjectInTile area objects in grid
        foreach (SaveData obj in areaToShow.Objects)
        {
            DungeonTile tile = _grid.GetElement(obj.Position) as DungeonTile;
            putObjectInTile(obj.Name, tile);
        }

        // update current area index
        _currentShownAreaPosition = position;
    }
    // ================================================================================================== //
    public void ClickSave()
    {
        /*string path = Application.dataPath + "/dungeons";

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);*/

        _dungeonSaveData.Name = _inputField.text;
        Utils.WriteToBinaryFile(Consts.DUNGEON_FILES_PATH + "/" + _inputField.text + ".dat", _dungeonSaveData);
    }
    // ================================================================================================== //
    public void ClickLoad()
    {
        _dungeonSaveData = Utils.ReadFromBinaryFile<DungeonSaveData>(Consts.DUNGEON_FILES_PATH + "/" + _inputField.text + ".dat");
        showArea(Position.OriginPosition);
    }
    // ================================================================================================== //
    private void checkRightClickOnTile()
    {
        if (!Input.GetMouseButton(1))
            return;

        LayerMask layerMask = (1 << LayerMask.NameToLayer("DungeonTile"));
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.0F, layerMask);
        if (hit.collider == null)
            return;

        DungeonTile targetTile = hit.collider.GetComponent<DungeonTile>();
        if (targetTile == null)
            return;

        targetTile.Clear();

        updateCurrentAreaSaveData();
    }
    // ================================================================================================== //
    private void checkLeftClickOnTile()
    {
        if (!Input.GetMouseButton(0))
            return;

        LayerMask layerMask = (1 << LayerMask.NameToLayer("DungeonTile"));
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.0F, layerMask);
        if (hit.collider == null)
            return;

        DungeonTile targetTile = hit.collider.GetComponent<DungeonTile>();
        if (targetTile == null)
            return;

        if (targetTile.transform.childCount > 0)
            return;

        if (ObjectSelector.Instance.SelectedObjectName == "")
            return;

        putObjectInTile(ObjectSelector.Instance.SelectedObjectName, targetTile);

        updateCurrentAreaSaveData();
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
    private void clear()
    {
        foreach (DungeonTile tile in _grid.Elements)
            tile.Clear();
    }
    // ================================================================================================== //
    public void ShowAreaInDirection(string directionStr)
    {
        AreaSaveData currentArea = getArea(_currentShownAreaPosition);

        Direction direction = Utils.GetDirectionByName(directionStr);

        Position areaInDirectionPosition = currentArea.Position.GetPositionInDirection(direction);

        showArea(areaInDirectionPosition);
    }
    // ================================================================================================== //
    private void updateCurrentAreaSaveData()
    {
        AreaSaveData currentArea = getArea(_currentShownAreaPosition);

        // clear all before save
        currentArea.Objects.Clear();

        foreach (DungeonTile tile in _grid.Elements)
        {
            foreach (Transform obj in tile.transform)
            {
                SaveData objData = obj.GetComponent<DungeonObject>().GetSaveData();
                objData.Position = tile.Position;
                currentArea.Objects.Add(objData);
            }
        }
    }
    // ================================================================================================== //
}
