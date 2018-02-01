using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/Item", order = 2)]
public class ItemData : ScriptableObject
{
    public Sprite Icon;
    public Sprite Image;
    public StatsDictionary Stats = new StatsDictionary();
    public SocketType SocketType;
    public ItemType Type;
}
