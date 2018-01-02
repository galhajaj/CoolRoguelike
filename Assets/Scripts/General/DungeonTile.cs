using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonTile : GridElement
{
    public bool IsBlockPath
    {
        get { return isContainBlockPathDungeonObjects(); }
    }
    public bool IsBlockView
    {
        get { return isContainBlockViewDungeonObjects(); }
    }

    public Item[] Items
    {
        get { return this.transform.GetComponentsInChildren<Item>(); }
    }

    public bool IsPortal
    {
        get { return (this.transform.GetComponentInChildren<Portal>() != null); }
    }
    public string LeadTo
    {
        get
        {
            if (!IsPortal)
                return "";
            return this.transform.GetComponentInChildren<Portal>().LeadTo;
        }
    }
    // ======================================================================================================================================== //
    private bool isContainBlockPathDungeonObjects() 
	{
        foreach (DungeonObject obj in this.transform.GetComponentsInChildren<DungeonObject>())
        {
            if (obj.IsBlockPath)
                return true;
        }

        return false;
	}
    // ======================================================================================================================================== //
    private bool isContainBlockViewDungeonObjects()
    {
        foreach (DungeonObject obj in this.transform.GetComponentsInChildren<DungeonObject>())
        {
            if (obj.IsBlockView)
                return true;
        }

        return false;
    }
    // ======================================================================================================================================== //
    public void Clear()
    {
        // delete object inside tile
        foreach (Transform child in this.transform)
            Destroy(child.gameObject);
    }
    // ======================================================================================================================================== //
    public void AddObject(GameObject obj)
    {
        obj.transform.position = this.transform.position;
        obj.transform.parent = this.transform;
    }
	// ======================================================================================================================================== //
}
