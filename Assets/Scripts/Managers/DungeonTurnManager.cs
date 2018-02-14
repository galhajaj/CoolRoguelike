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

            // check is see party & starting to chase them
            if (!creature.IsChasing)
                if (SightAlgorithm.CanSee(creature.Position, Party.Instance.Position))
                    creature.IsChasing = true;

            // continue if not chasing
            if (!creature.IsChasing)
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
                DungeonTile nextTile = WalkingAlgorithm.GetNextPosition(creature.Position, Party.Instance.Position).DungeonTile;
                if (nextTile != null)
                {
                    Debug.Log(creature.name + " move");
                    Dungeon.Instance.PutDungeonObjectInTile(creature, nextTile);
                    creature.PayWalkCost();
                }
            }
        }

        // end of creatures turn...
        _creaturesTurn = false;
    }
    // ====================================================================================================== //
    private void playerTurn()
    {
        playerContinuesInput();
        playerTriggerInput();

        // check if all party members have no action units left = end turn
        if (!Party.Instance.IsContainActiveMember)
            finishPlayerTurn();
    }
    // ====================================================================================================== //
    private void playerContinuesInput()
    {
        // make sure for nice timly equal interval between inputs
        if (_timeToNextMove >= 0.0F)
        {
            _timeToNextMove -= Time.deltaTime;
            return;
        }
        _timeToNextMove = _moveInterval;

        // waiting for player input
        checkDirectionKeys();
        checkWaitKey();
    }
    private void playerTriggerInput()
    {
        // waiting for player input
        checkUsePortalKey();
        checkPickupKey();
        checkShootButton();
    }
    // ====================================================================================================== //
    private void finishPlayerTurn()
    {
        Debug.Log("finish player turn");
        _creaturesTurn = true;
        // refill action units
        Party.Instance.FillActionUnitsForNextTurn();
    }
    // ====================================================================================================== //
    // can be move/ attack/ open door/ open chest...
    private void checkDirectionKeys()
    {
        Position targetPosition = Party.Instance.Position;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Keypad8) || Input.GetKey(KeyCode.UpArrow))
            targetPosition = Party.Instance.Position.North;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Keypad2) || Input.GetKey(KeyCode.DownArrow))
            targetPosition = Party.Instance.Position.South;
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Keypad4) || Input.GetKey(KeyCode.LeftArrow))
            targetPosition = Party.Instance.Position.West;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Keypad6) || Input.GetKey(KeyCode.RightArrow))
            targetPosition = Party.Instance.Position.East;
        else if (Input.GetKey(KeyCode.Keypad7))
            targetPosition = Party.Instance.Position.NorthWest;
        else if (Input.GetKey(KeyCode.Keypad9))
            targetPosition = Party.Instance.Position.NorthEast;
        else if (Input.GetKey(KeyCode.Keypad1))
            targetPosition = Party.Instance.Position.SouthWest;
        else if (Input.GetKey(KeyCode.Keypad3))
            targetPosition = Party.Instance.Position.SouthEast;

        // return if no change in position
        if (targetPosition == Party.Instance.Position)
            return;

        // ################################################
        // leave to adjacent area at borders or leave to village if in origin area
        // TODO: need refacturing - leave to adjacent area in turn manager
        if (targetPosition.IsOutsideDungeonArea)
        {
            if (Dungeon.Instance.IsInOriginArea)
            {
                WindowManager.Instance.LoadWindow("Village");
                return;
            }
            else
            {
                // 1. show adjacent area
                if (targetPosition == Party.Instance.Position.North)
                    Dungeon.Instance.ShowArea(Direction.NORTH);
                else if (targetPosition == Party.Instance.Position.South)
                    Dungeon.Instance.ShowArea(Direction.SOUTH);
                else if (targetPosition == Party.Instance.Position.West)
                    Dungeon.Instance.ShowArea(Direction.WEST);
                else if (targetPosition == Party.Instance.Position.East)
                    Dungeon.Instance.ShowArea(Direction.EAST);
                else
                    return;
                // 2. update targetPosition to be legal in adjacent tile
                targetPosition = targetPosition.CyclicPosition;

                // TODO: consider the situation where a creature is on that tile... move it to another one
            }
        }
        // ################################################

        DungeonTile targetTile = Dungeon.Instance.GetTile(targetPosition);
        Creature targetCreature = targetTile.GetContainedCreature();
        Creature activePartyMember = Party.Instance.SelectedMember;

        // choose action by the tile content
        if (targetCreature != null) // attack if creature in there
        {
            activePartyMember.MeleeAttack(targetCreature);
        }
        else if (!targetTile.IsBlockPath) // move to that location if free
        {
            Debug.Log("party move");
            Dungeon.Instance.PutDungeonObjectInTile(Party.Instance.DungeonObject, targetTile);
            Party.Instance.PayWalkCost();
        }
    }
    // ====================================================================================================== //
    private void checkWaitKey()
    {
        if (!Input.GetKey(KeyCode.Keypad5))
            return;

        Party.Instance.WaitTurn();
    }
    // ====================================================================================================== //
    private void checkUsePortalKey() // stairs etc.
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        DungeonTile partyTile = Dungeon.Instance.GetTile(Party.Instance.Position);
        if (!partyTile.IsPortal)
            return;

        Dungeon.Instance.ShowArea(partyTile.LeadTo);
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
    private void checkShootButton()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        LayerMask layerMask = (1 << LayerMask.NameToLayer("DungeonTile"));
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.0F, layerMask);
        if (hit.collider == null)
            return;

        DungeonTile targetTile = hit.collider.GetComponent<DungeonTile>();
        Creature targetCreature = targetTile.GetContainedCreature();
        if (targetCreature == null)
            return;

        Creature activePartyMember = Party.Instance.SelectedMember;
        activePartyMember.RangedAttack(targetCreature);

        Debug.Log("SHOOT!!!");
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
    }
    // ====================================================================================================== //
    private void openChest(DungeonTile targetTile)
    {
        // imp
    }
    // ====================================================================================================== //
}
