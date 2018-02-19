using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField]
    private bool _isShownAtStart = false;
    public bool IsShownAtStart { get { return _isShownAtStart; } }

    private Vector2 _hidePosition; // original position

    void Awake()
    {
        _hidePosition = this.transform.position;
    }

    void Start()
    {

    }

    public void Hide()
    {
        this.transform.position = _hidePosition;
    }

    public void Show()
    {
        this.transform.position = WindowManager.Instance.ShowWindowPosition;
    }
}
