using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        // TODO: change that... need to be by clicking on book that contains that string
        SL.Dungeon.Load("Ancient_Castle_Level_1");
        SL.WindowManager.LoadWindow<Dungeon>();
        //SL.WindowManager.LoadWindow<Village>();
    }

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
