using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SurfaceType
{
    WALL,
    STAIRS,
    EMPTY
}

public class Surface : MonoBehaviour 
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

	private bool _isSelected = false;
	public bool IsSelected
	{
		get { return _isSelected; }
		set 
		{ 
			_isSelected = value; 
		}
	}

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
