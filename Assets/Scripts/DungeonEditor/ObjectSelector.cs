using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSelector : Singleton<ObjectSelector>
{
    public string ObjectsFolderName = "Stuff";
    public string SelectedObjectName { get; private set; }

    [SerializeField]
    private Grid _grid = null;

    [SerializeField]
    private Text _textSelected = null;


    void Start()
    {
        StartCoroutine(fillGridCoroutine());
    }

    IEnumerator fillGridCoroutine()
    {
        yield return new WaitForSeconds(0.25F);
        fillGridWithSelectedObjects();
    }

    void Update()
    {
        checkLeftClickOnTile();
    }

    // toggle between stuff/portals/items/creatures
    public void ToggleObjectType()
    {
        if (ObjectsFolderName == "Stuff")
            ObjectsFolderName = "Items";
        else if (ObjectsFolderName == "Items")
            ObjectsFolderName = "Creatures";
        else if (ObjectsFolderName == "Creatures")
            ObjectsFolderName = "Stuff";

        Debug.Log("Loading " + ObjectsFolderName);

        fillGridWithSelectedObjects();
    }

    private void fillGridWithSelectedObjects()
    {
        // clear all
        foreach (GridElement tile in _grid.Elements)
            if (tile.transform.childCount > 0)
                Destroy(tile.transform.GetChild(0).gameObject);

        // get all object from resources folder
        GameObject[] allObjects = Resources.LoadAll<GameObject>(ObjectsFolderName);

        // create instance of each object and place it inside the grid
        for (int i = 0; i < allObjects.Length; ++i)
        {
            GameObject objInstance = Instantiate(allObjects[i]);
            objInstance.transform.position = _grid.Elements[i].transform.position;
            objInstance.transform.parent = _grid.Elements[i].transform;
        }
    }

    private void checkLeftClickOnTile()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        LayerMask layerMask = (1 << LayerMask.NameToLayer("SelectorTile"));
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.0F, layerMask);
        if (hit.collider == null)
            return;

        SelectedObjectName = hit.collider.transform.GetChild(0).name.Split('(')[0].Trim();
        _textSelected.text = SelectedObjectName;
    }
}
