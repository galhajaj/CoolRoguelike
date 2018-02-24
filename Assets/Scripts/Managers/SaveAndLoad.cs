using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public abstract class SaveData
{
    public string Name;
    public Position Position;
}

[Serializable]
public class PlayerSaveData : SaveData
{
    public List<DungeonSaveData> Dungeons = new List<DungeonSaveData>();
}

[Serializable]
public class DungeonSaveData : SaveData
{
    public Dictionary<Position, AreaSaveData> Areas = new Dictionary<Position, AreaSaveData>();
}

[Serializable]
public class AreaSaveData : SaveData
{
    public List<SaveData> Objects = new List<SaveData>();
}

[Serializable]
public class StuffSaveData : SaveData
{
    // public Rotation... add possibility to rotate object in map
}

[Serializable]
public class CreatureSaveData : SaveData
{
    //public List<ItemSaveData> EquipedItems = new List<ItemSaveData>();
    public List<ItemSaveData> CarriedItems = new List<ItemSaveData>();
    public int Hearts;
}

[Serializable]
public class ItemSaveData : SaveData
{
    public ItemDurability Durability;
    public ItemCondition Condition;
}

[Serializable]
public class TreasureSaveData : SaveData
{

}

public class SaveAndLoad : Singleton<SaveAndLoad>
{
    public PlayerSaveData PlayerSaveData = new PlayerSaveData();

    // ================================================================================================== //
    public void GenerateNewSaveGame()
    {
        Debug.Log("Generate new save game...");

        // clean save data
        PlayerSaveData.Dungeons.Clear();

        // load all dungeon files into save data
        DirectoryInfo d = new DirectoryInfo(Consts.DUNGEON_FILES_PATH);
        FileInfo[] files = d.GetFiles("*.dat");
        foreach (FileInfo file in files)
        {
            // current dungeon data from file
            DungeonSaveData dungeonSaveData = Utils.ReadFromBinaryFile<DungeonSaveData>(Consts.DUNGEON_FILES_PATH + "/" + file.Name);

            // for all areas
            foreach (AreaSaveData areaSaveData in dungeonSaveData.Areas.Values)
            {
                List<ItemSaveData> itemsSaveDataFromTreasures = new List<ItemSaveData>();

                // for all objects inside area
                foreach (SaveData objSaveData in areaSaveData.Objects)
                {
                    // for creature - add items from its defined treasure
                    if (objSaveData is CreatureSaveData)
                    {
                        CreatureSaveData creatureSaveData = objSaveData as CreatureSaveData;

                        // generate items from creature treasures
                        List<ItemSaveData> itemsSaveData = Treasure.GenerateItems(objSaveData as CreatureSaveData);
                        creatureSaveData.CarriedItems.AddRange(itemsSaveData);
                    }
                    // for item - update items parameters
                    else if (objSaveData is ItemSaveData)
                    {
                        ItemSaveData itemSaveData = objSaveData as ItemSaveData;
                        Item.AddRandomness(ref itemSaveData);
                    }
                    // random treasure - turn random treasure objects to actual items
                    else if (objSaveData is TreasureSaveData)
                    {
                        // generate items save data from treasure and add them to area
                        List<ItemSaveData> itemsSaveData = Treasure.GenerateItems(objSaveData as TreasureSaveData);
                        foreach (ItemSaveData itemSaveData in itemsSaveData)
                        {
                            itemSaveData.Position = objSaveData.Position;
                            itemsSaveDataFromTreasures.Add(itemSaveData);
                        }
                    }
                }

                // add all items generated from treasures objects
                foreach (ItemSaveData itemSaveData in itemsSaveDataFromTreasures)
                    areaSaveData.Objects.Add(itemSaveData);

                // delete all random treasure objects in area (they are just templates, the items from them already created)
                areaSaveData.Objects.RemoveAll(s => s is TreasureSaveData);
            }

            // add it
            PlayerSaveData.Dungeons.Add(dungeonSaveData);
        }

        // save to file with player name
        Utils.WriteToBinaryFile(Consts.SAVE_FILES_PATH + "/" + Consts.CURRENT_PLAYER + ".sav", PlayerSaveData);

        // library
        Library.Instance.BuildLibrary();
    }
    // ================================================================================================== //
    public void Save()
    {
        Utils.WriteToBinaryFile(Consts.SAVE_FILES_PATH + "/" + Consts.CURRENT_PLAYER + ".sav", PlayerSaveData);
    }
    // ================================================================================================== //
    public void Load()
    {
        PlayerSaveData = Utils.ReadFromBinaryFile<PlayerSaveData>(Consts.SAVE_FILES_PATH + "/" + Consts.CURRENT_PLAYER + ".sav");

        // library
        Library.Instance.BuildLibrary();
    }
    // ================================================================================================== //
}
