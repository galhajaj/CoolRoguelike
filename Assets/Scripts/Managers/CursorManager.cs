using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : Singleton<CursorManager>
{

	void Start ()
    {
		
	}

    void Update()
    {

    }
	
	void LateUpdate ()
    {
        MiniatureManager.Instance.SetMiniatureImage(null);

        cursorOverCreature();
        cursorOverPortrait();
    }

    private void cursorOverCreature()
    {
        GameObject creatureObj = Utils.GetObjectUnderCursor("Creature");
        if (creatureObj == null)
            return;

        Creature creature = creatureObj.GetComponent<Creature>();
        MiniatureManager.Instance.SetMiniatureImage(creature.Image);
    }

    private void cursorOverPortrait()
    {
        GameObject portraitObj = Utils.GetObjectUnderCursor("Portrait");
        if (portraitObj == null)
            return;

        Portrait portrait = portraitObj.GetComponent<Portrait>();
        MiniatureManager.Instance.SetMiniatureImage(portrait.Creature.Image);
    }
}
