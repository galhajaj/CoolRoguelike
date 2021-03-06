﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : Singleton<WindowManager>
{
    public event Action Event_WindowLoaded;
    public event Action Event_BeforeWindowLoaded;

    // the position where the selected window move to be shown
    [SerializeField]
    private Vector2 _showWindowPosition = Vector2.zero;
    public Vector2 ShowWindowPosition { get { return _showWindowPosition; } }

    private List<Window> _windows = new List<Window>();

    private string _currentWindowName = "";
    public string CurrentWindowName { get { return _currentWindowName; } }
    public bool IsCurrentWindow(string windowName) { return _currentWindowName.Equals(windowName); }

    private string _previousWindowName = "";

    // ====================================================================================================== //
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
    // ====================================================================================================== //
    public void LoadWindow(string name)
    {
        Window selectedWindow = GetWindowByName(name);

        // check exist
        if (selectedWindow == null)
        {
            Debug.LogError("Window [" + name + "] doesn't exist!");
            return;
        }

        // update previous window name
        if (name != _currentWindowName) // prevent stuck in window
            _previousWindowName = _currentWindowName;

        // update current window name
        _currentWindowName = name;

        // raise before window loaded event
        Event_BeforeWindowLoaded();

        // hide all & show selected
        hideAllWindows();
        selectedWindow.Show();

        // raise window loaded event
        Event_WindowLoaded();
    }
    // ====================================================================================================== //
    public void LoadPreviousWindow()
    {
        this.LoadWindow(_previousWindowName);
    }
    // ====================================================================================================== //
    // if window not contain the tab buttons, the back button will load the previous window
    /*public bool IsCurrentWindowIsModal()
    {
        Window currentWindow = GetWindowByName(_currentWindowName);
        return !currentWindow.IsContainTabButtons;
    }*/
    // ====================================================================================================== //
    private void hideAllWindows()
    {
        foreach (var window in _windows)
            window.Hide();
    }
    // ====================================================================================================== //
    public Window GetWindowByName(string name)
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
    // ====================================================================================================== //
}
