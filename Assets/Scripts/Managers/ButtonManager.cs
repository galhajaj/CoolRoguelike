using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : Singleton<ButtonManager>
{
    [SerializeField]
    private GameObject _portarits = null;
    [SerializeField]
    private GameObject _tabButtons = null; // inventory, characterSheet, group, settings & treasure bag

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void SetPortraitsVisibility(bool isVisible)
    {
        _portarits.SetActive(isVisible);
    }

    public void SetTabButtonsVisibility(bool isVisible)
    {
        _tabButtons.SetActive(isVisible);
    }
}
