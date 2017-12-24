using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField]
    private List<Window> _windows = new List<Window>();

    void Awake()
    {
        // init windows list
        var windows = GameObject.FindObjectsOfType<Window>();
        for (int i = 0; i <windows.Length; ++i)
        {
            Window currentWindow = windows[i];
            _windows.Add(currentWindow);
        }
    }

    public void LoadWindow(string name)
    {
        Window selectedWindow = getWindowByName(name);

        // check exist
        if (selectedWindow == null)
        {
            Debug.LogError("Window [" + name + "] doesn't exist!");
            return;
        }

        // hide all & show selected
        hideAllWindows();
        selectedWindow.Show();
    }

    private void hideAllWindows()
    {
        foreach (var window in _windows)
            window.Hide();
    }

    private Window getWindowByName(string name)
    {
        Window window = null;
        for (int i = 0; i < _windows.Count; ++i)
        {
            if (_windows[i].gameObject.name == name)
            {
                window = _windows[i];
                break;
            }
        }
        return window;
    }
}
