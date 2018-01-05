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
        set { _maxHearts = value; Debug.Log("max hearts set " + this.name); Event_StatsUpdated(); }
    }
    [SerializeField]
    private int _hearts = 5;
    public int Hearts
    {
        get { return _hearts; }
        set { _hearts = value; Debug.Log("hearts set " + this.name); Event_StatsUpdated(); }
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
        set { _maxMana = value; Debug.Log("max mana set " + this.name); Event_StatsUpdated(); }
    }
    [SerializeField]
    private int _mana = 1;
    public int Mana
    {
        get { return _mana; }
        set { _mana = value; Debug.Log("mana set " + this.name); Event_StatsUpdated(); }
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
        set { _actionUnits = value; Debug.Log("action units set " + this.name); Event_StatsUpdated(); }
    }

    public int MeleeAttackCost;

    // =================================================================================== //
    public void MeleeAttack(Creature target, bool isPartyMember = false)
    {
        // pay action units
        //if (isPartyMember)
            ActionUnits -= MeleeAttackCost;
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
