using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : GameButton
{

    protected override void afterClicked()
    {
        // if the current window is modal (has no tab buttons) load the previous window
        if (WindowManager.Instance.IsCurrentWindowIsModal())
        {
            WindowManager.Instance.LoadPreviousWindow();
        }
        // otherwise, load the village or dungeon
        else
        {
            if (Party.Instance.IsInVillage)
                WindowManager.Instance.LoadWindow(Consts.WindowNames.VILLAGE);
            else
                WindowManager.Instance.LoadWindow(Consts.WindowNames.DUNGEON);

            // make tab buttons active again
            ButtonManager.Instance.ActivateAllTabButtons();
        }
    }
}
