using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Library : Singleton<Library>
{
    [SerializeField]
    private GenericGrid _grid = null;

    private Dictionary<string, List<int>> _bookAndItsPages = new Dictionary<string, List<int>>();

    // ================================================================================================== //
    public List<int> GetPagesOfBook(string bookName)
    {
        return _bookAndItsPages[bookName];
    }
    // ================================================================================================== //
    public void BuildLibrary()
    {
        // rebuild the books grid to match the number of dungeons
        int numberOfDungeons = SaveAndLoad.Instance.PlayerSaveData.Dungeons.Count;
        _grid.Rebuild(numberOfDungeons);

        // set dungeon for each book
        for (int i = 0; i < numberOfDungeons; ++i)
        {
            string dungeonName = SaveAndLoad.Instance.PlayerSaveData.Dungeons[i].Name;
            Debug.Log(dungeonName + " Loaded to book...");
            _grid.GetElement(i).GetComponent<LibraryBook>().DungeonName = dungeonName;

            // init _bookAndItsPages dictionary page list for the current dungeon
            _bookAndItsPages[dungeonName] = new List<int>();
            // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            // TODO: delete those init test pages in book...
            if (dungeonName == "a")
            {
                _bookAndItsPages[dungeonName].Add(1);
                _bookAndItsPages[dungeonName].Add(3);
                _bookAndItsPages[dungeonName].Add(7);
            }
            else if (dungeonName == "b")
            {
                _bookAndItsPages[dungeonName].Add(1);
                _bookAndItsPages[dungeonName].Add(3);
                _bookAndItsPages[dungeonName].Add(7);
                _bookAndItsPages[dungeonName].Add(12);
                _bookAndItsPages[dungeonName].Add(13);
                _bookAndItsPages[dungeonName].Add(16);
                _bookAndItsPages[dungeonName].Add(19);
            }
            // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        }
    }
    // ================================================================================================== //
    public void AddPageItem(Item item)
    {
        int page = Convert.ToInt32(Utils.GetCleanName(item.name).Split('_')[1]);
        string bookName = OpenBook.Instance.DungeonName;

        if (_bookAndItsPages[bookName].Contains(page))
        {
            Debug.LogError("already existed page [" + page + "] is trying to added to the book [" + bookName + "]");
            return;
        }

        _bookAndItsPages[bookName].Add(page);
        _bookAndItsPages[bookName] = _bookAndItsPages[bookName].OrderBy(o => o).ToList(); // sort

        // destroy the page item
        Destroy(item.gameObject);
    }
    // ================================================================================================== //
}
