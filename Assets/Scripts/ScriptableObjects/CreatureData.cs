using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreatureData", menuName = "Data/Creature", order = 3)]
public class CreatureData : ScriptableObject
{
    public Sprite Icon;
    public Sprite Image;
    public StatsDictionary Stats = new StatsDictionary();
}
