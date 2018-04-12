using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : Singleton<Party>
{
    public Action Event_PartyMemberSelected;

    // position in dungeon, by its tile parent
    public Position Position { get { return this.GetComponentInParent<DungeonTile>().Position; } }
    /*public Position Position
    {
        get
        {
            DungeonTile parentTile = this.transform.parent.GetComponent<DungeonTile>();
            Position position = new Position(parentTile.PosX, parentTile.PosY);
            return position;
        }
    }*/

    private string _location = Consts.WindowNames.VILLAGE;
    public string Loaction
    {
        get { return _location; }
        set { _location = value; }
    }

    public bool IsInVillage { get { return _location == Consts.WindowNames.VILLAGE; } }

    // peace mode - 
    // TODO: performance (can be triggered each turn by the monsters & can be only a bool)
    // NOTE: now this function is in DungeonTurnManager and it's lighter and fasterrr
    /*public bool IsInPeaceMode
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
    }*/

    private List<Creature> _members = new List<Creature>();
    public List<Creature> Members { get { return _members; } }

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
    public void ResetActionUnits()
    {
        foreach (var member in _members)
            member.ResetActionUnits();
    }
    // ====================================================================================================== //
    public void RecoverOneTurnActionUnits()
    {
        foreach (var member in _members)
            member.RecoverOneTurnActionUnits();
    }
    // ====================================================================================================== //
    public void ExecuteEndTurnForTemporaryEffectItem()
    {
        foreach (Creature member in _members)
            member.ExecuteEndTurnForTemporaryEffectItem();
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
    // select the member after the selected member that is active
    // used for the turn manager where there is a situation of member not doing anything but still has action units
    // so its turn passes and the next member selected without returning to the previous
    public void SelectNextInTurnMember()
    {
        Creature selectedMember = this.SelectedMember;
        if (selectedMember == null)
            return;
        bool selectedFound = false;
        foreach (var member in _members)
        {
            if (!selectedFound)
                continue;

            if (member == selectedMember)
            {
                selectedFound = true;
                continue;
            }

            if (member.IsActive)
            {
                SelectedMember = member;
                return;
            }
        }
    }
    // ====================================================================================================== //
    public void PickupItemsInPosition()
    {
        DungeonTile partyTile = Dungeon.Instance.GetTile(this.Position);

        foreach (Item item in partyTile.Items)
        {
            // if book page - go straight to the library, otherwise - pick it up!
            if (item.Type == ItemType.BOOK_PAGE)
                Library.Instance.AddPageItem(item);
            else
                Inventory.Instance.AddItem(item);
        }
    }
    // ====================================================================================================== //
    public void UseStairsInPosition()
    {
        DungeonTile partyTile = Dungeon.Instance.GetTile(this.Position);
        if (!partyTile.IsPortal)
            return;

        Dungeon.Instance.ShowArea(partyTile.LeadTo);
        Dungeon.Instance.RevealPartySurroundings();
    }
    // ====================================================================================================== //
    public void MoveToAdjacentAreaInPosition()
    {
        // return if not in border
        if (!this.Position.IsBorder)
            return;

        // leave to town if in origin area and move behind its borders
        if (Dungeon.Instance.IsInOriginArea)
        {
            WindowManager.Instance.LoadWindow(Consts.WindowNames.VILLAGE);
            // update the location of the party to the village
            this.Loaction = Consts.WindowNames.VILLAGE;
        }
        else // go to adjacent area
        {
            if (this.Position.IsNorthBorder)
                Dungeon.Instance.ShowArea(Direction.NORTH);
            else if (this.Position.IsSouthBorder)
                Dungeon.Instance.ShowArea(Direction.SOUTH);
            else if (this.Position.IsWestBorder)
                Dungeon.Instance.ShowArea(Direction.WEST);
            else if (this.Position.IsEastBorder)
                Dungeon.Instance.ShowArea(Direction.EAST);

            Position targetPosition = this.Position.CyclicPosition;
            DungeonTile targetTile = Dungeon.Instance.GetTile(targetPosition);
            Dungeon.Instance.PutDungeonObjectInTile(this.DungeonObject, targetTile);
            Dungeon.Instance.RevealPartySurroundings();
            this.PayWalkCost();

            // TODO: when party move to adjacent area - handle situation where the place is blocked or contain monster
        }
    }
    // ====================================================================================================== //
}
