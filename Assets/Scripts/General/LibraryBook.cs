using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryBook : GameButton
{
    public override string Description
    {
        get
        {
            return DungeonName;
        }
    }

    public string DungeonName;

    protected override void beforeClicked()
    {
        OpenBook.Instance.DungeonName = DungeonName;
    }

    protected override void afterClicked()
    {
        
    }
}
