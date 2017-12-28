using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : Singleton<Party>
{
    // position in dungeon

    private Position _position;
    public Position Position
    {
        get { return _position; }
        set
        {
            _position = value;
            Dungeon.Instance.SetPartyMiniaturePositionInDungeon();
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
