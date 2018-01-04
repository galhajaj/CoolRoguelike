using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public event Action Event_StatsUpdated;
/*EnteredEndZone(endZone.EndZoneSide); // usage
    _ball.EnteredEndZone += BallOnEnteredEndZone; // assign to event in another class
private void BallOnEnteredEndZone(EndZone.EndZoneType endZoneType) // the function 
    {
        // do something
    }*/


    public string Name;

    // hearts
    [SerializeField]
    private int _maxHearts = 7;
    public int MaxHearts
    {
        get { return _maxHearts; }
        set { _maxHearts = value; Event_StatsUpdated(); }
    }
    [SerializeField]
    private int _hearts = 5;
    public int Hearts
    {
        get { return _hearts; }
        set { _hearts = value; Event_StatsUpdated(); }
    }

    public int MaxHitPoints;
    public int HitPoints;

    // mana
    [SerializeField]
    private int _maxMana = 3;
    public int MaxMana
    {
        get { return _maxMana; }
        set { _maxMana = value; Event_StatsUpdated(); }
    }
    [SerializeField]
    private int _mana = 1;
    public int Mana
    {
        get { return _mana; }
        set { _mana = value; Event_StatsUpdated(); }
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
        set { _actionUnits = value; Event_StatsUpdated(); }
    }

    public int MeleeAttackCost;

    public void PayMeleeAttackCost()
    {
        ActionUnits -= MeleeAttackCost;
    }
}
