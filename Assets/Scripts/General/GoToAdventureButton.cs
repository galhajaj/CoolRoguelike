using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToAdventureButton : GameButton
{

    protected override void afterClicked()
    {
        Debug.Log("Loading " + OpenBook.Instance.DungeonName + "...");
        Dungeon.Instance.Load(OpenBook.Instance.DungeonName);
        WindowManager.Instance.LoadWindow(Consts.WindowNames.DUNGEON);
    }
}
