using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorrowedBookButton : GameButton
{
    protected override void beforeClicked()
    {
        // update borrowed book name
        OpenBook.Instance.DungeonName = Party.Instance.BorrowedBookName;
    }
}
