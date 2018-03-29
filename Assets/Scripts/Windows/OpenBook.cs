using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBook : Singleton<OpenBook>
{
    public string DungeonName;

    [SerializeField]
    private GenericGrid _pagesGrid = null;

    [SerializeField]
    private TextMesh _pageText = null;

    [SerializeField]
    private GoToAdventureButton _goToAdventureButton = null;
    [SerializeField]
    private BackButton _backButton = null;

    // ================================================================================================== //
    void Start ()
    {
        WindowManager.Instance.Event_BeforeWindowLoaded += onBeforeWindowLoaded;
    }
    // ================================================================================================== //
    void Update ()
    {
		
	}
    // ================================================================================================== //
    public void DisplayPage(int pageNumber)
    {
        BookStory bookStory = Resources.Load<BookStory>("BookStories/" + DungeonName);
        _pageText.text = bookStory.Story[pageNumber - 1];

        // inactivate selected page (and activate the others)
        foreach (GridElement bookElement in _pagesGrid.Elements)
        {
            BookPage bookPage = bookElement.GetComponent<BookPage>();
            bookPage.IsActive = (bookPage.PageNumber != pageNumber);
        }
    }
    // ================================================================================================== //
    private void onBeforeWindowLoaded()
    {
        if (!WindowManager.Instance.IsCurrentWindow(Consts.WindowNames.OPEN_BOOK))
            return;

        // go to adventure button
        _goToAdventureButton.gameObject.SetActive(Party.Instance.IsInVillage);

        // back button
        _backButton.Type = Party.Instance.IsInVillage ? BackButton.BackButtonType.BACK_TO_PREVIOUS : BackButton.BackButtonType.BACK_TO_HOME;

        // update pages grid
        List<int> pages = Library.Instance.GetPagesOfBook(DungeonName);
        _pagesGrid.Rebuild(pages.Count);
        for (int i = 0; i < _pagesGrid.Elements.Count; ++i)
        {
            GridElement pageGridElement = _pagesGrid.Elements[i];
            pageGridElement.GetComponent<BookPage>().PageNumber = pages[i];
        }

        // show first page
        DisplayPage(pages[0]);
    }
    // ================================================================================================== //
}
