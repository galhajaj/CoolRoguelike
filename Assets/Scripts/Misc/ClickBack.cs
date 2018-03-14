using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickBack : MonoBehaviour
{
    void OnMouseDown()
    {
        if (Party.Instance.Loaction == Consts.WINDOW_VILLAGE)
            WindowManager.Instance.LoadWindow(Consts.WINDOW_VILLAGE);
        else
            WindowManager.Instance.LoadWindow(Consts.WINDOW_DUNGEON);
    }
}
