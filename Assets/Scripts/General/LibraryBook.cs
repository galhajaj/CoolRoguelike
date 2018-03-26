using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryBook : GameButton
{
    public string DungeonName;

    protected override void beforeClicked()
    {
        OpenBook.Instance.DungeonName = DungeonName;
    }

    protected override void afterClicked()
    {
        
    }
}
