using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPage : GameButton
{
    public int PageNumber;

    protected override void afterClicked()
    {
        OpenBook.Instance.DisplayPage(PageNumber);
    }
}
