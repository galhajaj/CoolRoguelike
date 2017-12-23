using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField]
    private List<Window> _windows = new List<Window>();

    void Awake()
    {
        // init windows list & deactivate them
        var windows = GameObject.FindObjectsOfType<Window>();
        for (int i = 0; i <windows.Length; ++i)
        {
            Window currentWindow = windows[i];
            _windows.Add(currentWindow);
            currentWindow.gameObject.SetActive(false);
        }
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void LoadWindow<T>()
    {
        // get window by type
        Window loadedWindow = null;
        for(int i = 0; i < _windows.Count; ++i)
        {
            if (_windows[i].gameObject.GetComponent<T>() != null)
            {
                loadedWindow = _windows[i];
            }
        }

        // check exist
        if (loadedWindow == null)
        {
            Debug.LogError("Window [" + name + "] doesn't exist!");
            return;
        }

        // deacivate all & activate selected
        foreach (var window in _windows)
            window.gameObject.SetActive(false);
        loadedWindow.gameObject.SetActive(true);
    }
}
