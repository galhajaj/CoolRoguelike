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
    public List<ItemSaveData> EquipedItems;
    public List<ItemSaveData> CarriedItems;
    public int Hearts;
}

[Serializable]
public class ItemSaveData : SaveData
{
    public ItemDurability Durability;
    public ItemCondition Condition;
}

[Serializable]
public class RandomTreasureSaveData : SaveData
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
                // for all objects inside area
                foreach (SaveData objSaveData in areaSaveData.Objects)
                {
                    // TODO: for creature - add items from its defined treasure
                    if (objSaveData is CreatureSaveData)
                    {
                        // imp
                    }
                    // for item - update items parameters
                    else if (objSaveData is ItemSaveData)
                    {
                        ItemSaveData itemSaveData = objSaveData as ItemSaveData;
                        itemSaveData.Durability = getRandomItemDurability();
                        itemSaveData.Condition = getRandomItemCondition();
                    }
                    // TODO: random treasure - turn random treasure objects to actual items
                    else if (objSaveData is RandomTreasureSaveData)
                    {
                        // imp
                    }
                }

                // delete all random treasure objects in area (they are just templates, the items from them already created)
                areaSaveData.Objects.RemoveAll(s => s is RandomTreasureSaveData);
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
    private ItemDurability getRandomItemDurability()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand < 2)
            return ItemDurability.UNBREAKABLE; // 2%
        if (rand < 7)
            return ItemDurability.FRAGILE; // 5%
        if (rand < 12)
            return ItemDurability.EXCELLENT; // 5%
        if (rand < 27)
            return ItemDurability.BAD; // 15%
        if (rand < 42)
            return ItemDurability.GOOD; // 15%
        return ItemDurability.NORMAL; // 58%
    }
    // ================================================================================================== //
    private ItemCondition getRandomItemCondition()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand < 10)
            return ItemCondition.BROKEN; // 10%
        if (rand < 30)
            return ItemCondition.DAMAGED; // 20%
        if (rand < 60)
            return ItemCondition.SLIGHTLY_DAMAGED; // 30%
        return ItemCondition.OK; // 40%
    }
    // ================================================================================================== //
}
