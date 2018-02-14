using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTreasure : DungeonObject
{
    public List<string> Categories;

    [Range(0,100)]
    public int Quality; // the percentage to add each bonus to item

    public override SaveData GetSaveData()
    {
        RandomTreasureSaveData saveData = new RandomTreasureSaveData();
        saveData.Name = Utils.GetCleanName(gameObject.name);
        return saveData;
    }
}
