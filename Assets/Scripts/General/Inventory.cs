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

    public int Copper = 0;
    private List<GameObject> currencyObjects = new List<GameObject>();

    // =================================================================================== //
    void Start()
    {
        _inventoryWidth = (int)_spriteRenderer.bounds.size.x;
        _inventoryHeight = (int)_spriteRenderer.bounds.size.y;

        AddRandomItems(_randomItemsNumber);
    }
    // =================================================================================== //
    public void AddItem(Item item)
    {
        addItemInPosition(item, getRandomPositionInsideInventory());
    }
    // =================================================================================== //
    private void addItemInPosition(Item item, Vector3 pos, int maxRadius = 0)
    {
        item.transform.parent = this.transform;

        Vector3 distanceFromCenter = Random.insideUnitCircle * Random.Range(0, maxRadius);
        distanceFromCenter = new Vector3((int)distanceFromCenter.x, (int)distanceFromCenter.y, (int)distanceFromCenter.z);

        item.transform.position = pos + distanceFromCenter;

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

            AddItem(randomItem);
        }

        ReorderOutOfBorderItems();
    }
    // =================================================================================== //
    private Vector3 getRandomPositionInsideInventory(int offset = 0)
    {
        int halfInventoryWidth = (_inventoryWidth - offset) / 2;
        int halfInventoryHeight = (_inventoryHeight - offset) / 2;

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

        // the currency borders are in the bottom of the cheat
        int currencyOffsetX = (itemTransform.GetComponent<Item>().Type == ItemType.CURRENCY) ? 120 : 0;
        int currencyOffsetY = (itemTransform.GetComponent<Item>().Type == ItemType.CURRENCY) ? 120 : 0;

        int halfInventoryWidth = (_inventoryWidth - itemWidth - currencyOffsetX) / 2;
        int halfInventoryHeight = (_inventoryHeight - itemHeight - currencyOffsetY) / 2;

        if (itemTransform.localPosition.x < -halfInventoryWidth)
            itemTransform.localPosition = new Vector3(-halfInventoryWidth, itemTransform.localPosition.y, itemTransform.localPosition.z);
        if (itemTransform.localPosition.x > halfInventoryWidth)
            itemTransform.localPosition = new Vector3(halfInventoryWidth, itemTransform.localPosition.y, itemTransform.localPosition.z);
        if (itemTransform.localPosition.y < -halfInventoryHeight)
            itemTransform.localPosition = new Vector3(itemTransform.localPosition.x, -halfInventoryHeight, itemTransform.localPosition.z);
        if (itemTransform.localPosition.y > halfInventoryHeight)
            itemTransform.localPosition = new Vector3(itemTransform.localPosition.x, halfInventoryHeight, itemTransform.localPosition.z);
    }
    // =================================================================================== //
    public void AddCurrency(GameObject currencyPrefab, int amount)
    {
        Vector3 randomPosition = getRandomPositionInsideInventory(120);

        for (int i = 0; i < amount; ++i)
        {
            GameObject currencyinstance = Instantiate(currencyPrefab);
            Item currencyItem = currencyinstance.GetComponent<Item>();
            this.Copper += currencyItem.ValueInCopper;
            addItemInPosition(currencyItem, randomPosition, amount * amount * amount);
            currencyObjects.Add(currencyinstance);
        }
    }
    // =================================================================================== //
    public void RemoveCopper(int amount)
    {
        if (amount > Copper)
        {
            amount = Copper;
            Debug.LogError("The amount of gold to remove is more than the current holdings...");
        }

        this.Copper -= amount;

        // TODO: implement correctly the remove currency objects in copper function
        for (int i = amount - 1; i >= 0; --i)
        {
            Destroy(currencyObjects[i].gameObject);
            currencyObjects.RemoveAt(i);
        }
    }
    // =================================================================================== //
}
