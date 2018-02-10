using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerSaveData
{
    public List<DungeonSaveData> Dungeons = new List<DungeonSaveData>();
}

[Serializable]
public class DungeonSaveData
{
    public string Name;
    public Dictionary<Position, AreaSaveData> Areas = new Dictionary<Position, AreaSaveData>();
}

[Serializable]
public class AreaSaveData
{
    public string Name;
    public Position Position;
    public List<StuffSaveData> Stuff = new List<StuffSaveData>();
    public List<CreatureSaveData> Creatures = new List<CreatureSaveData>();
    public List<ItemSaveData> Items = new List<ItemSaveData>();
}

[Serializable]
public class StuffSaveData
{
    public string Name;
    public Position Position;
    // public Rotation... add possibility to rotate object in map
}

[Serializable]
public class CreatureSaveData
{
    public string Name;
    public Position Position;
    public List<ItemSaveData> EquipedItems;
    public List<ItemSaveData> CarriedItems;
}

[Serializable]
public class ItemSaveData
{
    public string Name;
    public Position Position;
}

public class SaveAndLoad : Singleton<SaveAndLoad>
{
    public PlayerSaveData PlayerSaveData = new PlayerSaveData();

    public void GenerateNewSaveGame()
    {
        // clean save data
        PlayerSaveData.Dungeons.Clear();

        // load all dungeon files into save data
        DirectoryInfo d = new DirectoryInfo(Consts.DUNGEON_FILES_PATH);
        FileInfo[] files = d.GetFiles("*.dat");
        foreach (FileInfo file in files)
        {
            DungeonSaveData dungeonSaveData = Utils.ReadFromBinaryFile<DungeonSaveData>(Consts.DUNGEON_FILES_PATH + "/" + file.Name);
            PlayerSaveData.Dungeons.Add(dungeonSaveData);
        }

        // save to file with player name
        this.Save();

        // library
        Library.Instance.BuildLibrary();
    }

    public void Save()
    {
        Utils.WriteToBinaryFile(Consts.SAVE_FILES_PATH + "/" + Consts.CURRENT_PLAYER + ".sav", PlayerSaveData);
    }

    public void Load()
    {
        PlayerSaveData = Utils.ReadFromBinaryFile<PlayerSaveData>(Consts.SAVE_FILES_PATH + "/" + Consts.CURRENT_PLAYER + ".sav");

        // library
        Library.Instance.BuildLibrary();
    }
}
