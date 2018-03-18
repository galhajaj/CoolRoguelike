using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class University : MonoBehaviour
{
    [SerializeField]
    private GenericGrid _grid = null;

    public List<string> Skills = new List<string>();

	void Start ()
    {
        WindowManager.Instance.Event_WindowLoaded += universityWindowLoaded;
        Party.Instance.Event_PartyMemberSelected += universityWindowLoaded;
    }
	
	void Update ()
    {
        
	}

    private void universityWindowLoaded()
    {
        if (!WindowManager.Instance.IsCurrentWindow(Consts.WindowNames.UNIVERSITY))
            return;

        _grid.Rebuild(Skills.Count);

        for (int i = 0; i < Skills.Count; ++i)
        {
            Sprite skillSprite = Resources.Load<Sprite>("Skills/" + Skills[i]);
            _grid.GetElement(i).transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = skillSprite;
            // match the level to the current player stat 
            Stat stat = (Stat)Enum.Parse(typeof(Stat), Skills[i]); // get Stat by name
            _grid.GetElement(i).transform.Find("Level").GetComponent<TextMesh>().text = Party.Instance.SelectedMember.Stats[stat].ToString();
        }
    }
}
