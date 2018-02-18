using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click2NewGame : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("New Game...");

        SaveAndLoad.Instance.GenerateNewSaveGame();
        WindowManager.Instance.LoadWindow(Consts.VILLAGE);
    }
}
