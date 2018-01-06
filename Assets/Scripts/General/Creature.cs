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

    // items
    public Item Helmet;
    public Item Mail;
    public Item Armor;
    public Item Cape;
    public Item RightHand;
    public Item LeftHand;
    public Item Boots;
    public Item Ranged;
    public Item Ammo; // for ranged weapon...
    public Item Necklace;
    public List<Item> Rings = new List<Item>(); // for rings...

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
}
