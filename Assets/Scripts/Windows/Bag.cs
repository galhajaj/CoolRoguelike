using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bag : Singleton<Bag>
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer = null;

    private int _inventoryWidth;
    private int _inventoryHeight;

    public int Copper = 0;
    private List<Item> _currencyItems = new List<Item>();

    private const int X_OFFSET = 120;
    private const int Y_OFFSET = 120;

    // =================================================================================== //
    void Start()
    {
        _inventoryWidth = (int)_spriteRenderer.bounds.size.x;
        _inventoryHeight = (int)_spriteRenderer.bounds.size.y;
    }
    // =================================================================================== //
    void Update()
    {
        tempFunctionToAddCurrency_DELETE_ME();
    }
    // =================================================================================== //
    private void tempFunctionToAddCurrency_DELETE_ME()
    {
        // TODO: delete this line - add 100 gold pieces to party
        if (Input.GetKeyDown(KeyCode.U))
        {
            AddCurrency(ResourcesManager.Instance.GoldCoinPrefab, 3);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            AddCurrency(ResourcesManager.Instance.SilverCoinPrefab, 3);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            AddCurrency(ResourcesManager.Instance.CopperCoinPrefab, 3);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            AddCurrency(ResourcesManager.Instance.RubyPrefab, 3);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveCopper(25);
        }
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
    private Vector3 getRandomPositionInsideInventory()
    {
        int halfInventoryWidth = (_inventoryWidth - X_OFFSET) / 2;
        int halfInventoryHeight = (_inventoryHeight - Y_OFFSET) / 2;

        return this.transform.position + new Vector3(
                Random.Range(-halfInventoryWidth, halfInventoryWidth),
                Random.Range(-halfInventoryHeight, halfInventoryHeight));
    }
    // =================================================================================== //
    // all child items of inventory will get inside its borders
    public void ReorderOutOfBorderItems()
    {
        foreach (Item item in this.transform.GetComponentsInChildren<Item>())
        {
            orderItem(item.transform);
        }

        /*foreach (Transform itemTransform in this.transform)
        {
            orderItem(itemTransform);
        }*/
    }
    // =================================================================================== //
    private void orderItem(Transform itemTransform)
    {
        int itemWidth = (int)itemTransform.GetComponent<Collider2D>().bounds.size.x;
        int itemHeight = (int)itemTransform.GetComponent<Collider2D>().bounds.size.y;

        int halfInventoryWidth = (_inventoryWidth - itemWidth - X_OFFSET) / 2;
        int halfInventoryHeight = (_inventoryHeight - itemHeight - Y_OFFSET) / 2;

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
        Vector3 randomPosition = getRandomPositionInsideInventory();

        for (int i = 0; i < amount; ++i)
        {
            GameObject currencyinstance = Instantiate(currencyPrefab);
            Item currencyItem = currencyinstance.GetComponent<Item>();
            this.Copper += currencyItem.ValueInCopper;
            addItemInPosition(currencyItem, randomPosition, amount * amount * amount * amount * amount);
            _currencyItems.Add(currencyItem);
        }

        // sort by value, from higher to lower (to save game objects)
        _currencyItems = _currencyItems.OrderByDescending(o => o.ValueInCopper).ToList();
    }
    // =================================================================================== //
    public void RemoveCopper(int amount)
    {
        if (amount > Copper)
        {
            amount = Copper;
            Debug.LogError("The amount of gold to remove is more than the current holdings...");
        }

        for (int i = _currencyItems.Count - 1; i >= 0; --i)
        {
            Item item = _currencyItems[i];
            // if the current currency is lower or equal - remove its copper value and destroy it
            if (item.ValueInCopper <= amount)
            {
                this.Copper -= item.ValueInCopper;
                amount -= item.ValueInCopper;
                Destroy(item.gameObject);
                _currencyItems.RemoveAt(i);
            }
            // if the current currency is higher... we need change
            else
            {
                int change = item.ValueInCopper - amount;

                this.Copper -= item.ValueInCopper;
                amount -= item.ValueInCopper;
                Destroy(item.gameObject);
                _currencyItems.RemoveAt(i);

                // add coins until get the change back
                while (change > 0)
                {
                    if (change >= 100)
                    {
                        AddCurrency(ResourcesManager.Instance.GoldCoinPrefab, 1);
                        change -= 100;
                        continue;
                    }
                    if (change >= 10)
                    {
                        AddCurrency(ResourcesManager.Instance.SilverCoinPrefab, 1);
                        change -= 10;
                        continue;
                    }
                    if (change >= 1)
                    {
                        AddCurrency(ResourcesManager.Instance.CopperCoinPrefab, 1);
                        change -= 1;
                        continue;
                    }
                }

                return;
            }

            if (amount <= 0)
                return;
        }
    }
    // =================================================================================== //
}
