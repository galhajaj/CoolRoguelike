using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : GameButton
{
    public enum BackButtonType
    {
        BACK_TO_HOME, // village or dungeon
        BACK_TO_PREVIOUS
    }

    public BackButtonType Type = BackButtonType.BACK_TO_HOME;

    protected override void afterClicked()
    {
        if (Type == BackButtonType.BACK_TO_HOME)
        {
            if (Party.Instance.IsInVillage)
                WindowManager.Instance.LoadWindow(Consts.WindowNames.VILLAGE);
            else
                WindowManager.Instance.LoadWindow(Consts.WindowNames.DUNGEON);
        }
        else if (Type == BackButtonType.BACK_TO_PREVIOUS)
        {
            WindowManager.Instance.LoadPreviousWindow();
        }
    }
}
