using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public event Action Event_StatsUpdated;

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
        set { _maxHearts = value;
            if (Event_StatsUpdated != null) Event_StatsUpdated(); }
    }
    [SerializeField]
    private int _hearts = 5;
    public int Hearts
    {
        get { return _hearts; }
        set { _hearts = value;
            if (Event_StatsUpdated != null) Event_StatsUpdated(); }
    }

    public bool IsAlive { get { return Hearts > 0; } }

    public int MaxHitPoints;
    public int HitPoints;

    // mana
    [SerializeField]
    private int _maxMana = 3;
    public int MaxMana
    {
        get { return _maxMana; }
        set { _maxMana = value;
            if (Event_StatsUpdated != null) Event_StatsUpdated(); }
    }
    [SerializeField]
    private int _mana = 1;
    public int Mana
    {
        get { return _mana; }
        set { _mana = value;
            if (Event_StatsUpdated != null) Event_StatsUpdated(); }
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
        set { _actionUnits = value;
            if (Event_StatsUpdated != null) Event_StatsUpdated(); }
    }

    public int MeleeAttackCost;

    private Dictionary<ItemType, Item> _equippedItems = new Dictionary<ItemType, Item>();

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
}
