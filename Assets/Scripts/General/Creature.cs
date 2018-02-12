using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public Position Position { get { return this.GetComponentInParent<DungeonTile>().Position; } }

    [SerializeField]
    private DungeonObject _dungeonObject = null;
    public DungeonObject DungeonObject { get { return _dungeonObject; } }

    public GameObject Treasure;

    public StatsDictionary Stats = new StatsDictionary();

    public bool IsAlive { get { return Stats[Stat.HEARTS] > 0; } }

    // if monster chasing party
    private bool _isChasing = false;
    public bool IsChasing
    {
        get { return _isChasing; }
        set { _isChasing = value; }
    }

    // action units
    [SerializeField]
    private int _actionUnits = 12;
    public int ActionUnits
    {
        get { return _actionUnits; }
        set { _actionUnits = value; }
    }

    // is active = has action units left
    public bool IsActive { get { return _actionUnits > 0; } }

    public int WalkCost { get { return Consts.MAX_ACTION_UNITS; } }
    public int MeleeAttackCost { get { return Consts.MAX_ACTION_UNITS; } }
    public int RangedAttackCost { get { return Consts.MAX_ACTION_UNITS; } }

    // equipped items
    private Dictionary<SocketType, Item> _equippedItems = new Dictionary<SocketType, Item>();
    public Dictionary<SocketType, Item> EquippedItems { get { return _equippedItems; } }

    // items on belt
    private Dictionary<int, Item> _itemsOnBelt = new Dictionary<int, Item>();
    public Dictionary<int, Item> ItemsOnBelt { get { return _itemsOnBelt; } }

    // =================================================================================== //
    public void MeleeAttack(Creature target)
    {
        // pay action units
        this.ActionUnits -= MeleeAttackCost;
        // check if hit
        int rand = UnityEngine.Random.Range(0, 101);
        if (rand <= target.Stats[Stat.ARMOR])
        {
            Debug.Log(this.name + " missed " + target.name);
            return;
        }
        // hit
        target.Stats[Stat.HEARTS]--;
        Debug.Log(this.name + " hit " + target.name);
    }
    // =================================================================================== //
    public void RangedAttack(Creature target)
    {
        // pay action units
        this.ActionUnits -= RangedAttackCost;
        // check if hit
        int rand = UnityEngine.Random.Range(0, 101);
        if (rand <= target.Stats[Stat.ARMOR])
        {
            Debug.Log(this.name + " missed " + target.name);
            return;
        }
        // hit
        target.Stats[Stat.HEARTS]--;
        Debug.Log(this.name + " hit " + target.name);
    }
    // =================================================================================== //
    public void PayWalkCost()
    {
        this.ActionUnits -= WalkCost;
    }
    // =================================================================================== //
    public void EquipItem(Item item)
    {
        // remove if item type exists
        if (_equippedItems.ContainsKey(item.SocketType))
            RemoveItem(_equippedItems[item.SocketType]);

        // put in miniature
        MiniatureManager.Instance.AddItem(item);

        // add it
        _equippedItems[item.SocketType] = item;

        // add stats
        this.Stats += item.Stats;
        /*foreach (Stat stat in item.Stats.Keys)
            this.Stats[stat] += item.Stats[stat];*/
    }
    // =================================================================================== //
    public void RemoveItem(Item item)
    {
        // remove from map
        _equippedItems.Remove(item.SocketType);

        // put in inventory
        Inventory.Instance.AddItem(item);

        // remove stats
        foreach (Stat stat in item.Stats.Keys)
            this.Stats[stat] -= item.Stats[stat];
    }
    // =================================================================================== //
    public void InsertItemToBelt(Item item, GameObject beltSocket)
    {
        int beltSocketIndex = beltSocket.GetComponent<GridElement>().Index;

        //Debug.Log("INDEX: " + beltSocketIndex);

        // remove if item type exists
        if (_itemsOnBelt.ContainsKey(beltSocketIndex))
            if (_itemsOnBelt[beltSocketIndex] != null)
                RemoveItemFromBelt(_itemsOnBelt[beltSocketIndex]);

        // put in belt slot
        BeltManager.Instance.AddItem(item, beltSocket);

        // add it
        _itemsOnBelt[beltSocketIndex] = item;
    }
    // =================================================================================== //
    public void RemoveItemFromBelt(Item item)
    {
        // remove from map
        GridElement beltSocket = item.transform.parent.GetComponent<GridElement>();
        _itemsOnBelt[beltSocket.Index] = null;

        // put in inventory
        Inventory.Instance.AddItem(item);
    }
    // =================================================================================== //
}
