using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library : Singleton<Library>
{
    [SerializeField]
    private GenericGrid _grid = null;

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
            _grid.GetElement(i).GetComponent<GameButton>().LoadDungeonOnClick = dungeonName;
        }
    }
}
