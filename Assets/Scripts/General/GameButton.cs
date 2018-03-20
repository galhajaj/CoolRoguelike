using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButton : MonoBehaviour
{
    [SerializeField]
    private Color _mouseOverColor = Color.yellow;
    [SerializeField]
    private Color _buttonDownColor = Color.gray;
    [SerializeField]
    private Color _buttonInactiveColor = Color.red;

    private Color _originalColor;

    [SerializeField]
    private string _loadWindowOnClick = "";

    public string LoadDungeonOnClick = "";

    private bool _isActive = true;
    public bool IsActive
    {
        get { return _isActive; }
        set
        {
            _isActive = value;
            _spriteRenderer.color = _isActive ? _originalColor : _buttonInactiveColor;
        }
    }

    [SerializeField]
    private bool _isPushButton = false;

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    private bool _isMouseDownOnMe = false;

    // ====================================================================================================== //
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _originalColor = _spriteRenderer.color;
    }
    // ====================================================================================================== //
    void Start ()
    {
		
	}
    // ====================================================================================================== //
    void Update ()
    {
		
	}
    // ====================================================================================================== //
    public void Show()
    {
        _spriteRenderer.enabled = true;
        _collider.enabled = true;
    }
    // ====================================================================================================== //
    public void Hide()
    {
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
    }
    // ====================================================================================================== //
    void OnMouseDown()
    {
        if (!IsActive)
            return;

        _isMouseDownOnMe = true;
    }
    // ====================================================================================================== //
    void OnMouseUpAsButton()
    {
        if (!IsActive)
            return;

        clicked();
        _isMouseDownOnMe = false;
    }
    // ====================================================================================================== //
    void OnMouseUp()
    {
        if (!IsActive)
            return;

        _isMouseDownOnMe = false;
    }
    // ====================================================================================================== //
    void OnMouseOver()
    {
        if (!IsActive)
            return;

        _spriteRenderer.color = _isMouseDownOnMe ? _buttonDownColor : _mouseOverColor;
    }
    // ====================================================================================================== //
    void OnMouseExit()
    {
        if (!IsActive)
            return;

        _spriteRenderer.color = _originalColor;
    }
    // ====================================================================================================== //
    private void clicked()
    {
        // back to original color
        _spriteRenderer.color = _originalColor;

        if (_isPushButton)
        {
            foreach (Transform buttonTransform in transform.parent)
                buttonTransform.GetComponent<GameButton>().IsActive = true;
            this.IsActive = false;
        }

        // open window
        if (_loadWindowOnClick != "")
        {
            WindowManager.Instance.LoadWindow(_loadWindowOnClick);
        }
        // load dungeon
        if (LoadDungeonOnClick != "")
        {
            Debug.Log("Loading " + LoadDungeonOnClick + "...");
            Dungeon.Instance.Load(LoadDungeonOnClick);
            WindowManager.Instance.LoadWindow(Consts.WindowNames.DUNGEON);
        }

        // trigger virtual function afterClicked
        afterClicked();
    }
    // ====================================================================================================== //
    protected virtual void afterClicked()
    {

    }
    // ====================================================================================================== //
}
