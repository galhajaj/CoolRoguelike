using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonObjectData", menuName = "Data/DungeonObject", order = 1)]
public class DungeonObjectData : ScriptableObject
{
    public Sprite Icon;
    public bool IsBlockPath;
    public bool IsBlockView;
    public bool IsBlockGhosts;
    public bool IsMovable;
}