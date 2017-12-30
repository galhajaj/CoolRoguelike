using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SurfaceType
{
    WALL,
    STAIRS,
    EMPTY
}

public class DungeonTile : MonoBehaviour 
{
    [SerializeField]
    private Sprite _emptySprite = null;
    [SerializeField]
    private Sprite _wallSprite = null;
    [SerializeField]
    private Sprite _stairsSprite = null;

    public int PosX;
	public int PosY;

    private SurfaceType _type = SurfaceType.EMPTY;
    public SurfaceType Type
    {
        get { return _type; }
        set
        {
            _type = value;
            setSurfaceSprite();
        }
    }

    // contains the party / creature / chest / wall etc.
    public bool IsContainObject
    {
        get { return this.transform.childCount > 0; }
    }

    private List<Item> _items = new List<Item>();

    private SpriteRenderer _spriteRenderer = null;

    // ======================================================================================================================================== //
    void Awake()
    {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    // ======================================================================================================================================== //
	void Start () 
	{

	}
	// ======================================================================================================================================== //
	void Update () 
	{
	
	}
	// ======================================================================================================================================== //
	private void setSurfaceSprite()
    {
        switch (_type)
        {
            case SurfaceType.WALL:
                _spriteRenderer.sprite = _wallSprite;
                break;
            case SurfaceType.STAIRS:
                _spriteRenderer.sprite = _stairsSprite;
                break;
            case SurfaceType.EMPTY:
                _spriteRenderer.sprite = _emptySprite;
                break;
            default:
                Debug.LogError("Unknown surface type " + _type.ToString());
                break;
        }
    }
	// ======================================================================================================================================== //
}
