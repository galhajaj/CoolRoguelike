using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : Singleton<MouseManager>
{
    [SerializeField]
    private Texture2D _defaultCursor = null;
    [SerializeField]
    private Texture2D _selectedPocketItemCursor = null;
    [SerializeField]
    private Texture2D _rangedCursor = null;
    [SerializeField]
    private Texture2D _meleeCursor = null;

    // ====================================================================================================== //
    void Start ()
    {
		
	}
    // ====================================================================================================== //
    // TODO: cursor manager to work with events rather than update...
    void Update()
    {
        if (Party.Instance.SelectedMember.SelectedPocketItem != null)
        {
            setCursor(_selectedPocketItemCursor);
        }
        else
        {
            setCursor(_defaultCursor);
        }
    }
    // ====================================================================================================== //
    private void setCursor(Texture2D texture)
    {
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.ForceSoftware);
    }
    // ====================================================================================================== //
}
