using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonTile : GridElement
{
    [SerializeField]
    private Sprite _blackFogSprite = null;
    [SerializeField]
    private Sprite _transparentFogSprite = null;
    [SerializeField]
    private SpriteRenderer _fogOfWarSpriteRenderer = null;

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

    // fog of war
    public enum TileFogStatus
    {
        UNREVEALED,
        REVEALED_BUT_NOT_IN_SIGHT,
        IN_SIGHT
    }

    private TileFogStatus _tileFogStatus = TileFogStatus.UNREVEALED;
    public TileFogStatus FogStatus
    {
        get { return _tileFogStatus; }
        set
        {
            _tileFogStatus = value;
            setFogOfWarSprite();
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
        // delete object inside tile, unless it's the Party or fogOfWar
        foreach (Transform child in this.transform)
            if (child.GetComponent<Party>() == null && child.name != "FogOfWar")
                Destroy(child.gameObject);
    }
    // ======================================================================================================================================== //
    public void AddObject(GameObject obj)
    {
        obj.transform.position = this.transform.position;
        obj.transform.parent = this.transform;
    }
    // ======================================================================================================================================== //
    private void setFogOfWarSprite()
    {
        switch (_tileFogStatus)
        {
            case TileFogStatus.UNREVEALED:
                _fogOfWarSpriteRenderer.sprite = _blackFogSprite;
                break;
            case TileFogStatus.REVEALED_BUT_NOT_IN_SIGHT:
                _fogOfWarSpriteRenderer.sprite = _transparentFogSprite;
                break;
            case TileFogStatus.IN_SIGHT:
                _fogOfWarSpriteRenderer.sprite = null;
                break;
            default:
                break;
        }
    }
    // ======================================================================================================================================== //
}
