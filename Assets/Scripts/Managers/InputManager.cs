using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
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

        if (dx == 0 && dy == 0)
            return;
        Position newPosition = new Position(Party.Instance.Position.X + dx, Party.Instance.Position.Y + dy);
        Dungeon.Instance.PlaceObject(Party.Instance.gameObject, newPosition);
	}
}
