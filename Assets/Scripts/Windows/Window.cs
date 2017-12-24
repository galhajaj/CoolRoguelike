using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    private const int WINDOW_POS_X = -72;
    private const int WINDOW_POS_Y = 26;

    private Vector2 _hidePosition; // original position
    private Vector2 _showPosition;

    void Awake()
    {
        _hidePosition = this.transform.position;
        _showPosition = new Vector2(WINDOW_POS_X, WINDOW_POS_Y);
    }

    public void Hide()
    {
        this.transform.position = _hidePosition;
    }

    public void Show()
    {
        this.transform.position = _showPosition;
    }
}
