using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : GameButton
{

    protected override void afterClicked()
    {
        WindowManager.Instance.LoadPreviousWindow();
    }
}
