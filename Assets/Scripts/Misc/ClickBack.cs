using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickBack : MonoBehaviour
{
    void OnMouseDown()
    {
        if (Party.Instance.Loaction == Consts.VILLAGE)
            WindowManager.Instance.LoadWindow(Consts.VILLAGE);
        else
            WindowManager.Instance.LoadWindow(Consts.DUNGEON);
    }
}
