using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : Singleton<CursorManager>
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
        /*if (Party.Instance.SelectedMember.SelectedPocketItem != null)
        {
            setCursorLala(_selectedPocketItemCursor);
        }
        else
        {
            setCursorLala(_defaultCursor);
        }*/
    }
    // ====================================================================================================== //
    private void setCursor(Sprite sprite)
    {
        Cursor.SetCursor(sprite.texture, Vector2.zero, CursorMode.ForceSoftware);
    }
    // ====================================================================================================== //
    private void setCursorLala(Texture2D texture)
    {
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
    }
    // ====================================================================================================== //
}
