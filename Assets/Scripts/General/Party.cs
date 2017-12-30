﻿using System.Collections;
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

    private List<Creature> _characters = new List<Creature>();

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}