using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : Singleton<Party>
{
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

    private string _location = "Village";
    public string Loaction
    {
        get { return _location; }
        set { _location = value; }
    }

    private List<Creature> _members = new List<Creature>();

    [SerializeField]
    private DungeonObject _dungeonObject = null;
    public DungeonObject DungeonObject { get { return _dungeonObject; } }

    // ====================================================================================================== //
    override protected void AfterAwake()
    {
        // init members in list
        foreach (Creature member in this.transform.GetComponentsInChildren<Creature>())
            _members.Add(member);
    }
    // ====================================================================================================== //
    // get the next member (by order from left to tight) which has action units left
    public Creature GetActiveMember()
    {
        foreach (var member in _members)
            if (member.ActionUnits > 0)
                return member;
        return null;
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

        int rand = Random.Range(0, liveMembers.Count);
        return liveMembers[rand];
    }
    // ====================================================================================================== //
    public bool IsOutOfActionUnits()
    {
        // check if out of action units
        foreach (var member in _members)
            if (member.ActionUnits > 0)
                return false;
        return true;
    }
    // ====================================================================================================== //
    // add maximum amount, but not higher
    public void FillActionUnitsForNextTurn()
    {
        foreach (var member in _members)
        {
            member.ActionUnits += ConstsManager.Instance.MAX_ACTION_UNITS;
            // normalize to max
            if (member.ActionUnits > ConstsManager.Instance.MAX_ACTION_UNITS)
                member.ActionUnits = ConstsManager.Instance.MAX_ACTION_UNITS;
        }
    }
    // ====================================================================================================== //
    // init to maximum amount of action units
    public void RefillActionUnits()
    {
        foreach (var member in _members)
            member.ActionUnits = ConstsManager.Instance.MAX_ACTION_UNITS;
    }
    // ====================================================================================================== //
}
