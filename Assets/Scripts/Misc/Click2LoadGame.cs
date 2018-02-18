using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click2LoadGame : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("Load Game...");

        SaveAndLoad.Instance.Load();
        // TODO: maybe it will be in another place than the village... even inside a dungeon during battle
        WindowManager.Instance.LoadWindow(Consts.VILLAGE);
    }
}
