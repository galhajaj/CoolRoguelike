using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : Singleton<WindowManager>
{
    // the position where the selected window move to be shown
    [SerializeField]
    private Vector2 _showWindowPosition = Vector2.zero;
    public Vector2 ShowWindowPosition { get { return _showWindowPosition; } }

    private List<Window> _windows = new List<Window>();

    private string _currentWindowName = "";
    public bool IsCurrentWindow(string windowName) { return _currentWindowName.Equals(windowName); }

    protected override void AfterAwake()
    {
        // init windows list
        var windows = GameObject.FindObjectsOfType<Window>();
        for (int i = 0; i < windows.Length; ++i)
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

        // update current window name
        _currentWindowName = name;
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
