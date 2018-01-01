using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonInputManager : MonoBehaviour
{
    [SerializeField]
    private float _moveInterval = 0.75F;
    private float _timeToNextMove = 0.0F;

    private bool _creaturesTurn = true;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        // return if not in dungeon
        if (WindowManager.Instance.CurrentWindowName != "Dungeon")
            return;

        creaturesTurn();
        playerTurn();

        

        int dx = 0;
        int dy = 0;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            dy--;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            dy++;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            dx--;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            dx++;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            dy--;
            dx--;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            dy--;
            dx++;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            dy++;
            dx--;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            dy++;
            dx++;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            // imp. waiting
        }
        else if (Input.GetKeyDown(KeyCode.Space)) // stairs
        {
            DungeonTile partyTile = Dungeon.Instance.GetTile(Party.Instance.Position);
            if (partyTile.IsPortal)
            {
                if (partyTile.LeadTo == "Village")
                    WindowManager.Instance.LoadWindow(partyTile.LeadTo);

                Dungeon.Instance.Load(partyTile.LeadTo);
            }
        }
        else if (Input.GetKeyDown(KeyCode.P)) // pickup items
        {
            DungeonTile partyTile = Dungeon.Instance.GetTile(Party.Instance.Position);
            Inventory.Instance.AddItems(partyTile.Items);
        }

        // return if no change in position
        if (dx == 0 && dy == 0)
            return;

        Position newPosition = new Position(Party.Instance.Position.X + dx, Party.Instance.Position.Y + dy);
        DungeonTile newTile = Dungeon.Instance.GetTile(newPosition);

        // return if place not free
        if (newTile.IsBlockPath)
            return;

        Dungeon.Instance.SetDungeonObjectPosition(Party.Instance.DungeonObject, newPosition);
	}

    private void creaturesTurn()
    {
        if (!_creaturesTurn)
            return;

        // imp.
        // finish all the creatures action units

        _creaturesTurn = false;
    }

    private void playerTurn()
    {
        // make sure for nice timly equal interval between inputs
        if (_timeToNextMove >= 0.0F)
        {
            _timeToNextMove -= Time.deltaTime;
            return;
        }

        // waiting for player input
        // if all action units of all party has finished - then move to creatures turn
    }
}
