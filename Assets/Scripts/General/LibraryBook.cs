using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryBook : GridElement
{
    public string StoryName = "";
    public string DungeonName = "";

    private void OnMouseDown()
    {
        Debug.Log("Loading " + DungeonName + "...");
        Dungeon.Instance.Load(DungeonName);
        WindowManager.Instance.LoadWindow(Consts.DUNGEON);
    }
}
