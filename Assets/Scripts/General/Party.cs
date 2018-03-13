using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : Singleton<Party>
{
    public event Action Event_PartyMemberSelected;

    // position in dungeon, by its tile parent
    public Position Position
    {
        get
        {
            DungeonTile parentTile = this.transform.parent.GetComponent<DungeonTile>();
            Position position = new Position(parentTile.PosX, parentTile.PosY);
            return position;
        }
    }

    private string _location = Consts.VILLAGE;
    public string Loaction
    {
        get { return _location; }
        set { _location = value; }
    }

    // peace mode - 
    // TODO: performance (can be triggered each turn by the monsters & can be only a bool)
    public bool IsInPeaceMode
    {
        get
        {
            foreach (Creature creature in Dungeon.Instance.GetCreatures())
            {
                // continue if dead
                if (!creature.IsAlive)
                    continue;
                // if creature chasing... the party are during battle
                if (creature.IsChasing)
                    return false;
            }

            return true;
        }
    }

    private List<Creature> _members = new List<Creature>();

    private Creature _selectedMember = null;
    public Creature SelectedMember
    {
        get
        {
            if (_selectedMember != null)
                if (_selectedMember.IsActive)
                    return _selectedMember;
            foreach (var member in _members)
                if (member.IsActive)
                    return member;
            return null;
        }
        set
        {
            _selectedMember = value;
            Event_PartyMemberSelected();
        }
    }

    public bool IsContainActiveMember
    {
        get
        {
            foreach (var member in _members)
                if (member.IsActive)
                    return true;
            return false;
        }
    }

    [SerializeField]
    private DungeonObject _dungeonObject = null;
    public DungeonObject DungeonObject { get { return _dungeonObject; } }

    [SerializeField]
    private int _walkCost = 20;
    public int WalkCost { get { return _walkCost; } }

    // ====================================================================================================== //
    override protected void AfterAwake()
    {
        // init members in list
        foreach (Creature member in this.transform.GetComponentsInChildren<Creature>())
            _members.Add(member);
    }
    // ====================================================================================================== //
    public Creature GetRandomLiveMember()
    {
        // get list of all live members
        List<Creature> liveMembers = new List<Creature>();
        foreach (Creature member in _members)
            if (member.IsAlive)
                liveMembers.Add(member);

        if (liveMembers.Count == 0)
            return null;

        int rand = UnityEngine.Random.Range(0, liveMembers.Count);
        return liveMembers[rand];
    }
    // ====================================================================================================== //
    // add maximum amount, but not higher
    public void FillActionUnitsForNextTurn()
    {
        foreach (var member in _members)
        {
            member.ActionUnits += Consts.MAX_ACTION_UNITS;
            // normalize to max
            if (member.ActionUnits > Consts.MAX_ACTION_UNITS)
                member.ActionUnits = Consts.MAX_ACTION_UNITS;
        }
    }
    // ====================================================================================================== //
    // init to maximum amount of action units
    public void RefillActionUnits()
    {
        foreach (var member in _members)
            member.ActionUnits = Consts.MAX_ACTION_UNITS;
    }
    // ====================================================================================================== //
    public void PayWalkCost()
    {
        // party has shared walk cost
        foreach (var member in _members)
            member.ActionUnits -= WalkCost;
    }
    // ====================================================================================================== //
    public void WaitTurn()
    {
        foreach (var member in _members)
            member.ActionUnits -= Consts.MAX_ACTION_UNITS;
    }
    // ====================================================================================================== //
}
