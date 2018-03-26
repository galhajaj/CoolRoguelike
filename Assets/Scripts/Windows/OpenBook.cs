using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBook : Singleton<OpenBook>
{
    public string DungeonName;

    [SerializeField]
    private BorrowBookButton _borrowBookButton = null;
    [SerializeField]
    private GoToAdventureButton _goToAdventureButton = null;
    [SerializeField]
    private BackButton _backButton = null;

    void Start ()
    {
        WindowManager.Instance.Event_BeforeWindowLoaded += onBeforeWindowLoaded;

        // set default DungeonName to the party borrowed book
        DungeonName = Party.Instance.BorrowedBookName;
    }

    void Update ()
    {
		
	}

    private void onBeforeWindowLoaded()
    {
        if (!WindowManager.Instance.IsCurrentWindow(Consts.WindowNames.OPEN_BOOK))
            return;

        // borrow book button
        Debug.Log(Party.Instance.BorrowedBookName + " $$$ " + DungeonName);
        _borrowBookButton.gameObject.SetActive(Party.Instance.BorrowedBookName != DungeonName);

        // go to adventure button
        _goToAdventureButton.gameObject.SetActive(Party.Instance.IsInVillage && Party.Instance.BorrowedBookName == DungeonName);

        // back button
        _backButton.Type = Party.Instance.IsInVillage ? BackButton.BackButtonType.BACK_TO_PREVIOUS : BackButton.BackButtonType.BACK_TO_HOME;

        // tab buttons
        WindowManager.Instance.GetWindowByName(Consts.WindowNames.OPEN_BOOK).IsContainTabButtons = !Party.Instance.IsInVillage;
    }
}
