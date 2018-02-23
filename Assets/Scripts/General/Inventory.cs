﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    [SerializeField]
    private int _randomItemsNumber = 0;

    void Start()
    {
        AddRandomItems(_randomItemsNumber);
    }

    public void AddItems(Item[] items)
    {
        foreach (Item item in items)
        {
            AddItem(item);
        }
    }

    public void AddItem(Item item)
    {
        item.transform.position = this.transform.position;
        item.transform.parent = this.transform;
        item.State = ItemState.INVENTORY;
    }

    // TODO: fix inventory random placement to be precise
    public void AddRandomItems(int number)
    {
        SpriteRenderer inventoryRenderer = GetComponent<SpriteRenderer>();
        int inventoryWidth = (int)inventoryRenderer.size.x;
        int inventoryHeight = (int)inventoryRenderer.size.y;

        for (int i = 0; i < number; ++i)
        {
            GameObject[] allItems = Resources.LoadAll<GameObject>("Items/Standard");
            GameObject randomItemPrefab = allItems[Random.Range(0, allItems.Length)];
            GameObject randomItemInstance = Instantiate(randomItemPrefab);
            int itemWidth = (int)randomItemInstance.GetComponent<Collider2D>().bounds.size.x;
            int itemHeight = (int)randomItemInstance.GetComponent<Collider2D>().bounds.size.y;
            int halfInventoryWidth = (inventoryWidth - itemWidth) / 2;
            int halfInventoryHeight = (inventoryHeight - itemHeight) / 2;
            randomItemInstance.transform.parent = this.transform;
            randomItemInstance.transform.position = this.transform.position + new Vector3(
                Random.Range(-halfInventoryWidth, halfInventoryWidth),
                Random.Range(-halfInventoryHeight, halfInventoryHeight));
        }
    }
}
