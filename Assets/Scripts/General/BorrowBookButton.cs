using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorrowBookButton : GameButton
{
    protected override void afterClicked()
    {
        // update borrowed book name
        Party.Instance.BorrowedBookName = OpenBook.Instance.DungeonName;
        // move back to village
        WindowManager.Instance.LoadWindow(Consts.WindowNames.VILLAGE);
    }
}
