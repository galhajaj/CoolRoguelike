using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField]
    private bool _isShownAtStart = false;

    public bool IsContainTabButtons = true;
    public bool IsContainPortraits = true;

    private Vector2 _hidePosition; // original position

    void Awake()
    {
        
    }

    void Start()
    {
        _hidePosition = this.transform.position;
        if (_isShownAtStart)
            Show();
    }

    public void Hide()
    {
        this.transform.position = _hidePosition;
    }

    public void Show()
    {
        this.transform.position = WindowManager.Instance.ShowWindowPosition;
        
        // update caption with current window name
        TextDisplayer.Instance.SetMainCaption(this.name);

        // set buttons
        ButtonManager.Instance.SetTabButtonsVisibility(this.IsContainTabButtons);
        // set portraits
        ButtonManager.Instance.SetPortraitsVisibility(this.IsContainPortraits);
    }
}
