﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : DungeonObject
{
    [SerializeField]
    private bool isPartyMember = false;

    public Position Position { get { return this.GetComponentInParent<DungeonTile>().Position; } }

    //public new Sprite Image;

    public List<GameObject> Treasures;

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

    [SerializeField]
    private int _xPReward = 0;

    // carried items
    private List<Item> _carriedItems = new List<Item>();
    public List<Item> CarriedItems { get { return _carriedItems; } }

    // equipped items
    private Dictionary<SocketType, Item> _equippedItems = new Dictionary<SocketType, Item>();
    public Dictionary<SocketType, Item> EquippedItems { get { return _equippedItems; } }

    // items on belt
    private Dictionary<int, Item> _itemsInPockets = new Dictionary<int, Item>();
    public Dictionary<int, Item> ItemsInPockets { get { return _itemsInPockets; } }

    // selected pocket
    private int _selectedPocketIndex = -1;
    public int SelectedPocketIndex
    {
        get { return _selectedPocketIndex; }
        set { _selectedPocketIndex = value; }
    }

    public override SaveData GetSaveData()
    {
        CreatureSaveData saveData = new CreatureSaveData();
        saveData.Name = Utils.GetCleanName(gameObject.name);
        return saveData;
    }

    // =================================================================================== //
    // take damage and check for death
    public void TakeDamage(int amount, DamageType damageType)
    {
        // remove from hit points
        Stats[Stat.HIT_POINTS] -= amount;

        // if finish hit points - remove from hearts & init the hp
        if (Stats[Stat.HIT_POINTS] <= 0)
        {
            Stats[Stat.HIT_POINTS] = Stats[Stat.MAX_HIT_POINTS];
            Stats[Stat.HEARTS]--;
        }

        Debug.Log(name + " got " + amount.ToString() + " " + damageType.ToString() + "damage");

        // kill if not alive
        if (!IsAlive)
        {
            if (isPartyMember)
                killAsPartyMember();
            else
                killAsCreature();
        }
    }
    // =================================================================================== //
    private void killAsCreature()
    {
        // distribute xp points between alive party members
        foreach (Creature member in Party.Instance.Members)
            member.Stats[Stat.EXPERIENCE_POINTS] += _xPReward;

        // get last stand tile
        DungeonTile tile = this.Position.DungeonTile;

        // put all carried items on the ground
        foreach (Item item in CarriedItems)
        {
            item.Show();
            item.State = ItemState.GROUND;
            item.transform.position = tile.transform.position;
            item.transform.parent = tile.transform;
        }
        
        // destroy itself
        Destroy(gameObject);
    }
    // =================================================================================== //
    private void killAsPartyMember()
    {
        // TODO: implement killAsPartyMember()
    }
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
        int damage = UnityEngine.Random.Range(Stats[Stat.MIN_DAMAGE], Stats[Stat.MAX_DAMAGE] + 1);
        target.TakeDamage(damage, DamageType.PHYSICAL);

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
        int damage = UnityEngine.Random.Range(Stats[Stat.MIN_RANGED_DAMAGE], Stats[Stat.MAX_RANGED_DAMAGE] + 1);
        target.TakeDamage(damage, DamageType.PHYSICAL);

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
        // not equip if pocket type
        if (item.SocketType == SocketType.POCKET)
            return;

        // remove if item type exists
        if (_equippedItems.ContainsKey(item.SocketType))
            RemoveItem(_equippedItems[item.SocketType]);

        // change item state to equipped
        item.State = ItemState.EQUIPPED;

        // add it
        _equippedItems[item.SocketType] = item;

        // add stats
        this.Stats += item.Stats;
    }
    // =================================================================================== //
    public void RemoveItem(Item item)
    {
        // remove from map
        _equippedItems.Remove(item.SocketType);

        // put in inventory
        Inventory.Instance.AddItem(item);

        // remove stats
        this.Stats -= item.Stats;
    }
    // =================================================================================== //
    public void InsertItemInPocket(Item item, GameObject pocket)
    {
        // not equip if not pocket type
        if (item.SocketType != SocketType.POCKET)
            return;

        int beltSocketIndex = pocket.GetComponent<GridElement>().Index;

        //Debug.Log("INDEX: " + beltSocketIndex);

        // remove if item type exists
        if (_itemsInPockets.ContainsKey(beltSocketIndex))
            if (_itemsInPockets[beltSocketIndex] != null)
                RemoveItemFromPockets(_itemsInPockets[beltSocketIndex]);

        // put in belt slot
        PocketManager.Instance.AddItem(item, pocket);

        // add it
        _itemsInPockets[beltSocketIndex] = item;
    }
    // =================================================================================== //
    public void RemoveItemFromPockets(Item item)
    {
        // remove from map
        GridElement pocket = item.transform.parent.GetComponent<GridElement>();
        _itemsInPockets.Remove(pocket.Index);

        // put in inventory
        Inventory.Instance.AddItem(item);
    }
    // =================================================================================== //
}
