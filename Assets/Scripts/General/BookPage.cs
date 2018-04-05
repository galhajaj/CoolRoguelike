using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPage : GameButton
{
    [SerializeField]
    private TextMesh _textPageNumber = null;

    private int _pageNumber = 0;
    public int PageNumber
    {
        get { return _pageNumber; }
        set
        {
            _pageNumber = value;
            _textPageNumber.text = _pageNumber.ToString();
        }
    }

    protected override void afterClicked()
    {
        OpenBook.Instance.DisplayPage(PageNumber);
    }
}
