﻿using System;
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
    private InputField _inputField = null;

    private DungeonSaveData _dungeonSaveData = new DungeonSaveData();
    private Position _currentShownAreaPosition;

    void Start()
    {
        showArea(new Position(0, 0));
    }

    void Update()
    {
        checkLeftClickOnTile();
        checkRightClickOnTile();
    }

    private void showArea(Position position) // return the area index
    {
        // create new area if not exist
        if (!_dungeonSaveData.Areas.ContainsKey(position))
        {
            AreaSaveData newArea = new AreaSaveData();
            newArea.Position = position;
            _dungeonSaveData.Areas[position] = newArea;
        }

        // clear current grid
        clear();

        // get the area
        AreaSaveData areaToShow = _dungeonSaveData.Areas[position];

        // put area stuff in grid
        foreach (StuffSaveData stuff in areaToShow.Stuff)
        {
            DungeonTile tile = _grid.GetElement(stuff.Position) as DungeonTile;
            putObjectInTile("Stuff", stuff.Name, tile);
        }

        // update current area index
        _currentShownAreaPosition = position;
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
        //clear();
    }

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

        putObjectInTile(ObjectSelector.Instance.ObjectsFolderName, ObjectSelector.Instance.SelectedObjectName, targetTile);

        updateCurrentAreaSaveData();
    }

    private void putObjectInTile(string category, string objectName, DungeonTile targetTile)
    {
        string objectPath = category + "/" + objectName;
        GameObject prefabToCreate = Resources.Load<GameObject>(objectPath);
        if (prefabToCreate == null)
        {
            Debug.LogError(objectPath + " resource not exists");
            return;
        }

        GameObject instance = Instantiate(prefabToCreate);
        instance.transform.position = targetTile.transform.position;
        instance.transform.parent = targetTile.transform;
    }

    private void clear()
    {
        foreach (DungeonTile tile in _grid.Elements)
            tile.Clear();
    }

    public void ShowAreaInDirection(string direction)
    {
        AreaSaveData currentArea = _dungeonSaveData.Areas[_currentShownAreaPosition];

        if (direction == "North")
            showArea(currentArea.Position.Up);
        else if (direction == "South")
            showArea(currentArea.Position.Down);
        else if (direction == "West")
            showArea(currentArea.Position.Left);
        else if (direction == "East")
            showArea(currentArea.Position.Right);
        else if (direction == "Origin")
            showArea(new Position(0, 0));
        else
            Debug.LogError("not exist area direction: " + direction);
    }

    private void updateCurrentAreaSaveData()
    {
        AreaSaveData currentArea = _dungeonSaveData.Areas[_currentShownAreaPosition];

        currentArea.Stuff.Clear();

        foreach (DungeonTile tile in _grid.Elements)
        {
            foreach (Transform obj in tile.transform)
            {
                StuffSaveData stuff = new StuffSaveData();
                stuff.Name = obj.name.Split('(')[0].Trim();
                stuff.Position = tile.Position;
                currentArea.Stuff.Add(stuff);
            }
        }
    }
}
