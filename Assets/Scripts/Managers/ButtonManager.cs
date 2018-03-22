using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : Singleton<ButtonManager>
{
    [SerializeField]
    private GameObject _settingsButton = null;
    [SerializeField]
    private GameObject _tabButtons = null; // inventory, characterSheet & group

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void SetSettingsButtonVisibility(bool isVisible)
    {
        _settingsButton.SetActive(isVisible);
    }

    public void SetTabButtonsVisibility(bool isVisible)
    {
        _tabButtons.SetActive(isVisible);
    }

    public void ActivateAllTabButtons()
    {
        foreach (Transform buttonTransform in _tabButtons.transform)
        {
            buttonTransform.GetComponent<GameButton>().IsActive = true;
        }
    }
}
