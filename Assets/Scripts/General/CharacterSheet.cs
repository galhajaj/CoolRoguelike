using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheet : MonoBehaviour
{
    [SerializeField]
    private TextMesh _textCharacterName = null;
    [SerializeField]
    private TextMesh _textCharacterParams = null;

    void Start ()
    {
		
	}
	
	void Update ()
    {
        if (!WindowManager.Instance.IsCurrentWindow(Consts.CHARACTER_SHEET))
            return;

        // reference for the current displayed character
        Creature hero = Party.Instance.SelectedMember;

        _textCharacterName.text = hero.name;

        _textCharacterParams.text = "";
        // life, magic, weight & experience
        _textCharacterParams.text += "Life: " + hero.Stats[Stat.HEARTS] + "/" + hero.Stats[Stat.MAX_HEARTS] + 
            " [" + hero.Stats[Stat.HIT_POINTS] + "/" + hero.Stats[Stat.MAX_HIT_POINTS] + "]";
        _textCharacterParams.text += "\nMagic: " + hero.Stats[Stat.MANA] + "/" + hero.Stats[Stat.MAX_MANA];
        _textCharacterParams.text += "\nWeight: " + hero.Stats[Stat.WEIGHT];
        _textCharacterParams.text += "\nExperience Points: " + hero.Stats[Stat.EXPERIENCE_POINTS];

        // armor, to-hit, damage
        _textCharacterParams.text += "\n";
        _textCharacterParams.text += "\nArmor: " + hero.Stats[Stat.ARMOR] + "%";
        _textCharacterParams.text += "\nMelee: " + hero.Stats[Stat.MIN_DAMAGE] + "-" + hero.Stats[Stat.MAX_DAMAGE]  + " (" + hero.Stats[Stat.TO_HIT_MELEE] + "%)";
        _textCharacterParams.text += "\nRanged: " + hero.Stats[Stat.MIN_RANGED_DAMAGE] + "-" + hero.Stats[Stat.MAX_RANGED_DAMAGE]  + " (" + hero.Stats[Stat.TO_HIT_RANGED] + "%)";

        // attributes
        _textCharacterParams.text += "\n";
        _textCharacterParams.text += "\nStrength: " + hero.Stats[Stat.STRENGTH];
        _textCharacterParams.text += "\nEndurance: " + hero.Stats[Stat.ENDURANCE];
        _textCharacterParams.text += "\nAgility: " + hero.Stats[Stat.AGILITY];
        _textCharacterParams.text += "\nIntellect: " + hero.Stats[Stat.INTELLECT];
        _textCharacterParams.text += "\nWill: " + hero.Stats[Stat.WILL];
        _textCharacterParams.text += "\nAccuracy: " + hero.Stats[Stat.ACCURACY];

        // belt slots & rings
        _textCharacterParams.text += "\n";
        _textCharacterParams.text += "\nBelt Slots: " + hero.Stats[Stat.BELT_SLOTS_NUMBER];
        _textCharacterParams.text += "\nMax Rings Allowed: " + hero.Stats[Stat.MAX_RINGS_ALLOWED];
    }
}
