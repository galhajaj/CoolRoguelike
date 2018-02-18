using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click2LoadDungeon : MonoBehaviour
{
    [SerializeField]
    private string _dungeonName = "";

    void OnMouseDown()
    {
        Dungeon.Instance.Load(_dungeonName);
        WindowManager.Instance.LoadWindow(Consts.DUNGEON);
    }
}
