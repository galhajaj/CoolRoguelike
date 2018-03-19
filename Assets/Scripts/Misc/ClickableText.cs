using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickableText : MonoBehaviour
{

    [SerializeField]
    private Color _mouseOverColor = Color.yellow;
    [SerializeField]
    private Color _buttonDownColor = Color.gray;

    private Color _originalColor;

    [SerializeField]
    private string _loadWindowOnClick = "";

    [SerializeField]
    private string _loadSceneOnClick = "";

    [SerializeField]
    private bool _newGameOnClick = false;
    [SerializeField]
    private bool _loadGameOnClick = false;
    [SerializeField]
    private bool _exitGameOnClick = false;

    private TextMesh _textMesh;
    private Collider2D _collider;

    private bool _isMouseDownOnMe = false;

    // ====================================================================================================== //
    void Awake()
    {
        _textMesh = GetComponent<TextMesh>();
        _collider = GetComponent<Collider2D>();
        _originalColor = _textMesh.color;
    }
    // ====================================================================================================== //
    void Start()
    {

    }
    // ====================================================================================================== //
    void Update()
    {

    }
    // ====================================================================================================== //
    void OnMouseDown()
    {
        _isMouseDownOnMe = true;
    }
    // ====================================================================================================== //
    void OnMouseUpAsButton()
    {
        clicked();
        _isMouseDownOnMe = false;
    }
    // ====================================================================================================== //
    void OnMouseUp()
    {
        _isMouseDownOnMe = false;
    }
    // ====================================================================================================== //
    void OnMouseOver()
    {
        _textMesh.color = _isMouseDownOnMe ? _buttonDownColor : _mouseOverColor;
    }
    // ====================================================================================================== //
    void OnMouseExit()
    {
        _textMesh.color = _originalColor;
    }
    // ====================================================================================================== //
    private void clicked()
    {
        // open window
        if (_loadWindowOnClick != "")
        {
            WindowManager.Instance.LoadWindow(_loadWindowOnClick);
        }
        // load scene
        if (_loadSceneOnClick != "")
        {
            SceneManager.LoadScene(_loadSceneOnClick);
        }
        // new game
        if (_newGameOnClick)
        {
            Debug.Log("New Game...");
            SaveAndLoad.Instance.GenerateNewSaveGame();
            WindowManager.Instance.LoadWindow(Consts.WindowNames.VILLAGE);
        }
        // load game
        if (_loadGameOnClick)
        {
            Debug.Log("Load Game...");
            SaveAndLoad.Instance.Load();
            // TODO: maybe it will be in another place than the village... even inside a dungeon during battle
            WindowManager.Instance.LoadWindow(Consts.WindowNames.VILLAGE);
        }
        // exit game
        if (_exitGameOnClick)
        {
            Application.Quit();
        }
    }
    // ====================================================================================================== //
}
