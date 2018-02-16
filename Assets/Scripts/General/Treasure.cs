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

    public List<ItemSaveData> GenerateItems()
    {
        List<ItemSaveData> itemsSaveData = new List<ItemSaveData>();

        // check if create treasure
        if (!Utils.IsChanceOccured(Chance2Create))
            return itemsSaveData;

        foreach (string path in Paths)
        {
            // get rnadom item from current resources path
            GameObject[] itemPrefabs = Resources.LoadAll<GameObject>("Items/" + path);
            if (itemPrefabs.Length == 0)
                Debug.LogError("no prefabs inside path: " + path);
            GameObject randomItemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

            // check the chance to create the current item
            if (!Utils.IsChanceOccured(Chance2CreateEachMemebr))
                continue;

            GameObject itemObj = Instantiate(randomItemPrefab);

            ItemSaveData itemSaveData = itemObj.GetComponent<Item>().GetSaveData() as ItemSaveData;
            Item.AddRandomness(ref itemSaveData);
            itemsSaveData.Add(itemSaveData);

            Destroy(itemObj.gameObject);
        }

        return itemsSaveData;
    }
}
