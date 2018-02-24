using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : DungeonObject
{
    [Range(0, 100)]
    public int Chance2Create = 100;

    [Range(0, 100)]
    public int Chance2CreateEachMemebr = 100;

    // TODO: treasure magic properties - implement
    [Range(0, 100)]
    public int Chance4EachMagicProperty = 0;

    // TODO: treasure quality - implement
    public ItemQuality Quality = ItemQuality.NORMAL;

    public List<string> Paths;

    public override SaveData GetSaveData()
    {
        TreasureSaveData saveData = new TreasureSaveData();
        saveData.Name = Utils.GetCleanName(gameObject.name);
        return saveData;
    }

    // static function to generate items from treasure save data
    public static List<ItemSaveData> GenerateItems(TreasureSaveData saveData)
    {
        List<ItemSaveData> itemsSaveData = new List<ItemSaveData>();

        // get treasure object from resources
        GameObject treasurePrefab = Resources.Load<GameObject>("Treasures/" + saveData.Name);
        GameObject treasureObj = Instantiate(treasurePrefab);
        Treasure treasure = treasureObj.GetComponent<Treasure>();

        // check if create treasure
        if (!Utils.IsChanceOccured(treasure.Chance2Create))
            return itemsSaveData;

        foreach (string path in treasure.Paths)
        {
            // get rnadom item from current resources path
            GameObject[] itemPrefabs = Resources.LoadAll<GameObject>("Items/" + path);
            if (itemPrefabs.Length == 0)
                Debug.LogError("no prefabs inside path: " + path);
            GameObject randomItemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

            // check the chance to create the current item
            if (!Utils.IsChanceOccured(treasure.Chance2CreateEachMemebr))
                continue;

            GameObject itemObj = Instantiate(randomItemPrefab);

            ItemSaveData itemSaveData = itemObj.GetComponent<Item>().GetSaveData() as ItemSaveData;
            Item.AddRandomness(ref itemSaveData);
            itemsSaveData.Add(itemSaveData);

            Destroy(itemObj.gameObject);
        }

        Destroy(treasureObj.gameObject);
        return itemsSaveData;
    }

    // overloading for creature which is containing treasures...
    public static List<ItemSaveData> GenerateItems(CreatureSaveData saveData)
    {
        List<ItemSaveData> itemsSaveData = new List<ItemSaveData>();

        // get creature object from resources
        GameObject creaturePrefab = Resources.Load<GameObject>("Creatures/" + saveData.Name);
        GameObject creatureObj = Instantiate(creaturePrefab);
        Creature creature = creatureObj.GetComponent<Creature>();

        foreach (GameObject treasureObj in creature.Treasures)
        {
            Treasure treasure = treasureObj.GetComponent<Treasure>();
            List<ItemSaveData> itemsSaveDataFromTreasure = GenerateItems(treasure.GetSaveData() as TreasureSaveData);
            itemsSaveData.AddRange(itemsSaveDataFromTreasure);
        }

        Destroy(creatureObj.gameObject);
        return itemsSaveData;
    }
}
