using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSelector : Singleton<ObjectSelector>
{
    public string SelectedObjectName { get; private set; }

    [SerializeField]
    private GenericGrid _grid = null;

    [SerializeField]
    private Text _textSelected = null;

    private string _objectsFolderName = "Stuff";

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
        if (_objectsFolderName == "Stuff")
            _objectsFolderName = "Items";
        else if (_objectsFolderName == "Items")
            _objectsFolderName = "Creatures";
        else if (_objectsFolderName == "Creatures")
            _objectsFolderName = "Treasures";
        else if (_objectsFolderName == "Treasures")
            _objectsFolderName = "BookPages";
        else if (_objectsFolderName == "BookPages")
            _objectsFolderName = "Stuff";

        Debug.Log("Loading " + _objectsFolderName);

        fillGridWithSelectedObjects();
    }

    private void fillGridWithSelectedObjects()
    {
        // clear all
        foreach (GridElement tile in _grid.Elements)
            if (tile.transform.childCount > 0)
                Destroy(tile.transform.GetChild(0).gameObject);

        // get all object from resources folder
        GameObject[] allObjects = Resources.LoadAll<GameObject>(_objectsFolderName);

        // create instance of each object and place it inside the grid
        for (int i = 0; i < allObjects.Length; ++i)
        {
            GameObject objInstance = Instantiate(allObjects[i]);
            objInstance.transform.position = _grid.Elements[i].transform.position;
            objInstance.transform.parent = _grid.Elements[i].transform;
            // if item, make it in ground state, so the sprite will change to icon
            if (objInstance.GetComponent<Item>() != null)
                objInstance.GetComponent<Item>().State = ItemState.GROUND;
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

        SelectedObjectName = Utils.GetCleanName(hit.collider.transform.GetChild(0).name);
        _textSelected.text = SelectedObjectName;
    }
}
