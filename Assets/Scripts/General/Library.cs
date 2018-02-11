using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library : Singleton<Library>
{
    [SerializeField]
    private Grid _grid = null;

    public void BuildLibrary()
    {
        int numberOfDungeons = SaveAndLoad.Instance.PlayerSaveData.Dungeons.Count;
        _grid.Rebuild(numberOfDungeons);

        // TODO: add things for each book in library - so the dungeon of it will load
        for (int i = 0; i < numberOfDungeons; ++i)
        {
            string dungeonName = SaveAndLoad.Instance.PlayerSaveData.Dungeons[i].Name;
            Debug.Log(dungeonName + " Loaded to book...");
            _grid.GetElement(i).GetComponent<LibraryBook>().DungeonName = dungeonName;
        }
    }
}
