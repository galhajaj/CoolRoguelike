﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheet : MonoBehaviour
{
    [SerializeField]
    private TextMesh _textCharacterName = null;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        if (!WindowManager.Instance.IsCurrentWindow(Consts.CHARACTER_SHEET))
            return;

        _textCharacterName.text = Party.Instance.SelectedMember.name;
	}
}
