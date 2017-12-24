using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click2LoadDungeon : MonoBehaviour
{
    [SerializeField]
    private string _dungeonName = "";

    void OnMouseDown()
    {
        SM.Dungeon.Load(_dungeonName);
        SM.WindowManager.LoadWindow("Dungeon");
    }
}
