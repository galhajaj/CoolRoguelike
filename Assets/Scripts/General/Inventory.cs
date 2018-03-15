using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    [SerializeField]
    private int _randomItemsNumber = 0;

    [SerializeField]
    private SpriteRenderer _spriteRenderer = null;

    private int _inventoryWidth;
    private int _inventoryHeight;

    // =================================================================================== //
    void Start()
    {
        _inventoryWidth = (int)_spriteRenderer.bounds.size.x;
        _inventoryHeight = (int)_spriteRenderer.bounds.size.y;

        AddRandomItems(_randomItemsNumber);
    }
    // =================================================================================== //
    public void AddItems(Item[] items)
    {
        foreach (Item item in items)
        {
            AddItem(item);
        }
    }
    // =================================================================================== //
    public void AddItem(Item item, bool toRandomPlace = false)
    {
        item.transform.parent = this.transform;
        item.transform.position = toRandomPlace ? getRandomPositionInsideInventory() : this.transform.position;

        orderItem(item.transform);
        item.State = ItemState.INVENTORY;
    }
    // =================================================================================== //
    // TODO: fix inventory random placement to be precise
    public void AddRandomItems(int number)
    {
        for (int i = 0; i < number; ++i)
        {
            GameObject[] allItems = Resources.LoadAll<GameObject>("Items/Standard");
            GameObject randomItemPrefab = allItems[Random.Range(0, allItems.Length)];
            GameObject randomItemInstance = Instantiate(randomItemPrefab);
            Item randomItem = randomItemInstance.GetComponent<Item>();

            AddItem(randomItem, true);
        }

        ReorderOutOfBorderItems();
    }
    // =================================================================================== //
    private Vector3 getRandomPositionInsideInventory()
    {
        int halfInventoryWidth = (_inventoryWidth) / 2;
        int halfInventoryHeight = (_inventoryHeight) / 2;

        return this.transform.position + new Vector3(
                Random.Range(-halfInventoryWidth, halfInventoryWidth),
                Random.Range(-halfInventoryHeight, halfInventoryHeight));
    }
    // =================================================================================== //
    // all child items of inventory will get inside its borders
    public void ReorderOutOfBorderItems()
    {
        foreach (Transform itemTransform in this.transform)
        {
            orderItem(itemTransform);
        }
    }
    // =================================================================================== //
    private void orderItem(Transform itemTransform)
    {
        int itemWidth = (int)itemTransform.GetComponent<Collider2D>().bounds.size.x;
        int itemHeight = (int)itemTransform.GetComponent<Collider2D>().bounds.size.y;
        int halfInventoryWidth = (_inventoryWidth - itemWidth) / 2;
        int halfInventoryHeight = (_inventoryHeight - itemHeight) / 2;

        if (itemTransform.localPosition.x < -(_inventoryWidth - itemWidth) / 2)
            itemTransform.localPosition = new Vector3(-halfInventoryWidth, itemTransform.localPosition.y, itemTransform.localPosition.z);
        if (itemTransform.localPosition.x > _inventoryWidth / 2 - itemWidth / 2)
            itemTransform.localPosition = new Vector3(halfInventoryWidth, itemTransform.localPosition.y, itemTransform.localPosition.z);
        if (itemTransform.localPosition.y < -(_inventoryHeight - itemHeight) / 2)
            itemTransform.localPosition = new Vector3(itemTransform.localPosition.x, -halfInventoryHeight, itemTransform.localPosition.z);
        if (itemTransform.localPosition.y > (_inventoryHeight - itemHeight) / 2)
            itemTransform.localPosition = new Vector3(itemTransform.localPosition.x, halfInventoryHeight, itemTransform.localPosition.z);
    }
    // =================================================================================== //
}
