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
    public Direction LeadTo
    {
        get
        {
            if (!IsPortal)
                return Direction.NONE;
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
    public Creature GetContainedCreature()
    {
        // not include the party
        if (this.Position == Party.Instance.Position)
            return null;

        return this.transform.GetComponentInChildren<Creature>();
    }
    // ======================================================================================================================================== //
    public bool IsContainLoot
    {
        get { return this.transform.GetComponentsInChildren<Item>().Length > 0; }
    }
    // ======================================================================================================================================== //
    public Sprite GetImage()
    {
        foreach (DungeonObject obj in transform.GetComponentsInChildren<DungeonObject>())
        {
            if (obj.Image != null)
                return obj.Image;
        }

        return null;
    }
    // ======================================================================================================================================== //
    public void Clear()
    {
        // delete object inside tile, unless it's the Party
        foreach (Transform child in this.transform)
            if (child.GetComponent<Party>() == null)
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
