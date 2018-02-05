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
    private InputField _inputField = null;

    private DungeonSaveData _dungeonSaveData = new DungeonSaveData();

    void Start()
    {

    }

    void Update()
    {
        checkLeftClickOnTile();
        checkRightClickOnTile();
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

        string objectPath = ObjectSelector.Instance.ObjectsFolderName + "/" + ObjectSelector.Instance.SelectedObjectName;
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
}
