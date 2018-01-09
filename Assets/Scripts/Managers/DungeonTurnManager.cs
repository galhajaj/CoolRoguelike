using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTurnManager : MonoBehaviour
{
    [SerializeField]
    private float _moveInterval = 0.75F;
    private float _timeToNextMove = 0.0F;

    private bool _creaturesTurn = true;

	void Start ()
    {
		
	}
	// ====================================================================================================== //
	void Update ()
    {
        // return if not in dungeon
        if (WindowManager.Instance.CurrentWindowName != "Dungeon")
            return;

        creaturesTurn();
        playerTurn();
	}
    // ====================================================================================================== //
    private void creaturesTurn()
    {
        if (!_creaturesTurn)
            return;

        Debug.Log("Creatures turn");

        foreach(Creature creature in Dungeon.Instance.GetCreatures())
        {
            // continue if dead
            if (!creature.IsAlive)
                continue;

            // if close to party - melee
            if (creature.Position.DistanceTo(Party.Instance.Position) == 1)
            {
                Creature randomPartyMember = Party.Instance.GetRandomLiveMember();
                if (randomPartyMember != null)
                    creature.MeleeAttack(randomPartyMember);
            }
            else // otherwise, move toward party
            {
                DungeonTile nextTile = Dungeon.Instance.GetNextStupidTile(creature.Position, Party.Instance.Position);
                if (nextTile != null)
                {
                    move(creature.DungeonObject, nextTile);
                }
            }
        }

        // end of creatures turn...
        _creaturesTurn = false;
    }
    // ====================================================================================================== //
    private void playerTurn()
    {
        // make sure for nice timly equal interval between inputs
        if (_timeToNextMove >= 0.0F)
        {
            _timeToNextMove -= Time.deltaTime;
            return;
        }

        // waiting for player input
        checkDirectionKeys();
        checkWaitKey();
        checkUsePortalKey();
        checkPickupKey();
    }
    // ====================================================================================================== //
    private void finishPlayerTurn()
    {
        Debug.Log("finish player turn");
        _creaturesTurn = true;
    }
    // ====================================================================================================== //
    // can be move/ attack/ open door/ open chest...
    private void checkDirectionKeys()
    {
        Position targetPosition = Party.Instance.Position;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.UpArrow))
            targetPosition = Party.Instance.Position.Up;
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.DownArrow))
            targetPosition = Party.Instance.Position.Down;
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.LeftArrow))
            targetPosition = Party.Instance.Position.Left;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.RightArrow))
            targetPosition = Party.Instance.Position.Right;
        else if (Input.GetKeyDown(KeyCode.Keypad7))
            targetPosition = Party.Instance.Position.UpLeft;
        else if (Input.GetKeyDown(KeyCode.Keypad9))
            targetPosition = Party.Instance.Position.UpRight;
        else if (Input.GetKeyDown(KeyCode.Keypad1))
            targetPosition = Party.Instance.Position.DownLeft;
        else if (Input.GetKeyDown(KeyCode.Keypad3))
            targetPosition = Party.Instance.Position.DownRight;

        // return if no change in position
        if (targetPosition == Party.Instance.Position)
            return;

        DungeonTile targetTile = Dungeon.Instance.GetTile(targetPosition);
        Creature targetCreature = targetTile.GetContainedCreature();
        Creature activePartyMember = Party.Instance.SelectedMember;

        // choose action by the tile content
        if (targetCreature != null)
        {
            activePartyMember.MeleeAttack(targetCreature);
            // if all action units of all party has finished - then move to creatures turn
            if (Party.Instance.IsOutOfActionUnits())
            {
                finishPlayerTurn();
                Party.Instance.FillActionUnitsForNextTurn();
            }
        }
        else if (!targetTile.IsBlockPath)
        {
            move(Party.Instance.DungeonObject, targetTile);
            finishPlayerTurn();
        }
    }
    // ====================================================================================================== //
    private void checkWaitKey()
    {
        if (!Input.GetKeyDown(KeyCode.Keypad5))
            return;

        finishPlayerTurn();
    }
    // ====================================================================================================== //
    private void checkUsePortalKey() // stairs etc.
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        DungeonTile partyTile = Dungeon.Instance.GetTile(Party.Instance.Position);
        if (!partyTile.IsPortal)
            return;

        if (partyTile.LeadTo == "Village")
            WindowManager.Instance.LoadWindow("Village");

        Dungeon.Instance.Load(partyTile.LeadTo);
    }
    // ====================================================================================================== //
    private void checkPickupKey()
    {
        if (!Input.GetKeyDown(KeyCode.P))
            return;

        DungeonTile partyTile = Dungeon.Instance.GetTile(Party.Instance.Position);
        Inventory.Instance.AddItems(partyTile.Items);
    }
    // ====================================================================================================== //
    private void move(DungeonObject obj, DungeonTile tile)
    {
        Debug.Log(obj.name + " move");
        Dungeon.Instance.PutDungeonObjectInTile(obj, tile);
    }
    // ====================================================================================================== //
    private void rangedAttack(Creature attacker, Creature defender, DungeonTile targetTile)
    {
        // imp
    }
    // ====================================================================================================== //
    private void openDoor(DungeonTile targetTile)
    {
        // imp
        finishPlayerTurn(); // not relevant in peace mode...
    }
    // ====================================================================================================== //
    private void openChest(DungeonTile targetTile)
    {
        // imp
    }
    // ====================================================================================================== //
}
