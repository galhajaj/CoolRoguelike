using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
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
}
