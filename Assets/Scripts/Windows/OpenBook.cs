using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBook : Singleton<OpenBook>
{
    public string DungeonName;

    [SerializeField]
    private GoToAdventureButton _goToAdventureButton = null;
    [SerializeField]
    private BackButton _backButton = null;

    void Start ()
    {
        WindowManager.Instance.Event_BeforeWindowLoaded += onBeforeWindowLoaded;
    }

    void Update ()
    {
		
	}

    private void onBeforeWindowLoaded()
    {
        if (!WindowManager.Instance.IsCurrentWindow(Consts.WindowNames.OPEN_BOOK))
            return;

        // go to adventure button
        _goToAdventureButton.gameObject.SetActive(Party.Instance.IsInVillage);

        // back button
        _backButton.Type = Party.Instance.IsInVillage ? BackButton.BackButtonType.BACK_TO_PREVIOUS : BackButton.BackButtonType.BACK_TO_HOME;

        // tab buttons
        //WindowManager.Instance.GetWindowByName(Consts.WindowNames.OPEN_BOOK).IsContainTabButtons = !Party.Instance.IsInVillage;
    }
}
