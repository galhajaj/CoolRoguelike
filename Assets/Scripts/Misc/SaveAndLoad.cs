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
    public List<RandomTreasureSaveData> RandomTreasures = new List<RandomTreasureSaveData>();
}

/*[Serializable]
public class TiledSaveData
{
    public string Name;
    public Position Position;
}*/

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
    public int Hearts;
}

[Serializable]
public class ItemSaveData
{
    public string Name;
    public Position Position;
    public ItemDurability Durability;
    public ItemCondition Condition;
}

[Serializable]
public class RandomTreasureSaveData
{
    public string Name;
    public Position Position;
}

public class SaveAndLoad : Singleton<SaveAndLoad>
{
    public PlayerSaveData PlayerSaveData = new PlayerSaveData();

    // ================================================================================================== //
    public void GenerateNewSaveGame()
    {
        // clean save data
        PlayerSaveData.Dungeons.Clear();

        // load all dungeon files into save data
        DirectoryInfo d = new DirectoryInfo(Consts.DUNGEON_FILES_PATH);
        FileInfo[] files = d.GetFiles("*.dat");
        foreach (FileInfo file in files)
        {
            // current dungeon data from file
            DungeonSaveData dungeonSaveData = Utils.ReadFromBinaryFile<DungeonSaveData>(Consts.DUNGEON_FILES_PATH + "/" + file.Name);

            // update creatures & items
            foreach (AreaSaveData areaSaveData in dungeonSaveData.Areas.Values)
            {
                // creatures - add items from its defined treasure
                foreach (CreatureSaveData creatureSaveData in areaSaveData.Creatures)
                {

                }

                // update items parameters
                foreach (ItemSaveData itemSaveData in areaSaveData.Items)
                {
                    itemSaveData.Durability = getRandomItemDurability();
                    itemSaveData.Condition = getRandomItemCondition();
                }

                // turn random treasure objects to actual items
                foreach (RandomTreasureSaveData randomTreasureSaveData in areaSaveData.RandomTreasures)
                {

                }

                // clear the random treasure list, it's not needed anymore
                areaSaveData.RandomTreasures.Clear();
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
