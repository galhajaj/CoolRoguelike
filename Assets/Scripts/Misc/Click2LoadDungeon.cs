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
        Party.Instance.Position = new Position(7, 5); // TODO: this is temp, make it by the stairs name
        WindowManager.Instance.LoadWindow("Dungeon");
    }
}
