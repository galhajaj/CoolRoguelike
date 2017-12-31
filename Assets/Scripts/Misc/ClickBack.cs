using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickBack : MonoBehaviour
{
    void OnMouseDown()
    {
        if (Party.Instance.Loaction == "Village")
            WindowManager.Instance.LoadWindow("Village");
        else
            WindowManager.Instance.LoadWindow("Dungeon");
    }
}
