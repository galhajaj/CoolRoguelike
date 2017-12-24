using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click2OpenWindow : MonoBehaviour
{
    [SerializeField]
    private string _windowName = "";

    void OnMouseDown()
    {
        SM.WindowManager.LoadWindow(_windowName);
    }
}
