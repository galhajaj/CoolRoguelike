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

    override protected void AfterAwake()
    {
        // init members in list
        foreach (Creature member in this.transform.GetComponentsInChildren<Creature>())
            _members.Add(member);
    }

    public Creature GetRandomMember()
    {
        int rand = Random.Range(0, _members.Count);
        return _members[rand];
    }
}
