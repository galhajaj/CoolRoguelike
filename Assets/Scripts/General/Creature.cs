﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public string Name;

    public Position Position { get { return this.GetComponentInParent<DungeonTile>().Position; } }

    [SerializeField]
    private DungeonObject _dungeonObject = null;
    public DungeonObject DungeonObject { get { return _dungeonObject; } }

    // hearts
    [SerializeField]
    private int _maxHearts = 7;
    public int MaxHearts
    {
        get { return _maxHearts; }
        set { _maxHearts = value; }
    }
    [SerializeField]
    private int _hearts = 5;
    public int Hearts
    {
        get { return _hearts; }
        set { _hearts = value; }
    }

    public bool IsAlive { get { return _hearts > 0; } }

    public int MaxHitPoints;
    public int HitPoints;

    // mana
    [SerializeField]
    private int _maxMana = 3;
    public int MaxMana
    {
        get { return _maxMana; }
        set { _maxMana = value; }
    }
    [SerializeField]
    private int _mana = 1;
    public int Mana
    {
        get { return _mana; }
        set { _mana = value; }
    }

    public int MinDamage;
    public int MaxDamage;
    public int Shield;

    // action units
    [SerializeField]
    private int _actionUnits = 12;
    public int ActionUnits
    {
        get { return _actionUnits; }
        set { _actionUnits = value; }
    }

    // belt slots number (max - 5)
    [SerializeField]
    private int _beltSlotsCount = 3;
    public int BeltSlotsCount
    {
        get { return _beltSlotsCount; }
        set { _beltSlotsCount = value; }
    }

    // is active = has action units left
    public bool IsActive { get { return _actionUnits > 0; } }

    public int WalkCost;
    public int MeleeAttackCost;
    public int RangedAttackCost;

    // equipped items
    private Dictionary<ItemType, Item> _equippedItems = new Dictionary<ItemType, Item>();
    public Dictionary<ItemType, Item> EquippedItems { get { return _equippedItems; } }

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
        if (rand <= target.Shield)
        {
            Debug.Log(this.name + " missed " + target.name);
            return;
        }
        // hit
        target.Hearts--;
        Debug.Log(this.name + " hit " + target.name);
    }
    // =================================================================================== //
    public void RangedAttack(Creature target)
    {
        // pay action units
        this.ActionUnits -= RangedAttackCost;
        // check if hit
        int rand = UnityEngine.Random.Range(0, 101);
        if (rand <= target.Shield)
        {
            Debug.Log(this.name + " missed " + target.name);
            return;
        }
        // hit
        target.Hearts--;
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
        if (_equippedItems.ContainsKey(item.Type))
            RemoveItem(_equippedItems[item.Type]);

        // put in miniature
        MiniatureManager.Instance.AddItem(item);

        // add it
        _equippedItems[item.Type] = item;

        // add stats
        this.MaxHearts += item.MaxHearts;
        this.MaxMana += item.MaxMana;
    }
    // =================================================================================== //
    public void RemoveItem(Item item)
    {
        // remove from map
        _equippedItems.Remove(item.Type);

        // put in inventory
        Inventory.Instance.AddItem(item);

        // remove stats
        this.MaxHearts -= item.MaxHearts;
        this.MaxMana -= item.MaxMana;
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
