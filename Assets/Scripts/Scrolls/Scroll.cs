using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : Item
{
    public enum ScrollTargetType
    {
        NONE, // the scroll immediately activated when selected - no target nedded
        DUNGEON_TILE,
        CREATURE,
        PARTY_MEMBER
    }

    public ScrollTargetType TargetType = ScrollTargetType.NONE;
    public DungeonTile TargetDungeonTile;
    public Creature TargetCreature;

    [SerializeField]
    private int _manaCost = 1;
    [SerializeField]
    private int _actionUnitsCost = 15;

    public void Activate()
    {
        // check if has enough mana
        if (Party.Instance.SelectedMember.Stats[Stat.MANA] < _manaCost)
            return;

        

        // execute! (override in inherited child) - if return true (succeeded) pay the costs
        if (activate())
        {
            // pay mana
            Party.Instance.SelectedMember.Stats[Stat.MANA] -= _manaCost;
            // pay action units
            Party.Instance.SelectedMember.ActionUnits -= _actionUnitsCost;
        }
        // return if not succeeded... and not cancel selection of scroll (?)
        else
        {
            return;
        }

        //cancel selection of scroll // TODO: this is the place to cancel the selection of the scroll? maybe in checkUseSelectedPocketItem() in DungeonTurnManager?
        Party.Instance.SelectedMember.SelectedPocketItem = null;
    }
    protected virtual bool activate() { return true; }
}
