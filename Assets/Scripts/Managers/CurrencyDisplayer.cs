using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyDisplayer : Singleton<CurrencyDisplayer>
{
    [SerializeField]
    private TextMesh _textAmount = null;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        _textAmount.text = Inventory.Instance.Copper.ToString() + " Copper Coins";
    }
}
