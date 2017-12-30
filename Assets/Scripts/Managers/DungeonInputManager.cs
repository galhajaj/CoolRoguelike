using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonInputManager : MonoBehaviour
{
	void Start ()
    {
		
	}
	
	void Update ()
    {
        if (WindowManager.Instance.CurrentWindowName != "Dungeon")
            return;

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
            if (partyTile.Type == SurfaceType.STAIRS)
            {
                if (partyTile.LeadTo == "Village")
                    WindowManager.Instance.LoadWindow(partyTile.LeadTo);

                Dungeon.Instance.Load(partyTile.LeadTo);
            }
        }

        // return if no change in position
        if (dx == 0 && dy == 0)
            return;

        Position newPosition = new Position(Party.Instance.Position.X + dx, Party.Instance.Position.Y + dy);

        // return if place not free
        if (Dungeon.Instance.GetTile(newPosition).IsContainObject)
            return;

        Dungeon.Instance.PlaceObject(Party.Instance.gameObject, newPosition);
	}
}
