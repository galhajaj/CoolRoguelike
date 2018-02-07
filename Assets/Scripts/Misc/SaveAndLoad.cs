using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DungeonSaveData
{
    public string Name;
    public Dictionary<Position, AreaSaveData> Areas = new Dictionary<Position, AreaSaveData>();
}

[Serializable]
public class AreaSaveData
{
    public string Name;
    public Position Position;
    public List<StuffSaveData> Stuff = new List<StuffSaveData>();
    public List<CreatureSaveData> Creatures = new List<CreatureSaveData>();
    public List<ItemSaveData> Items = new List<ItemSaveData>();
}

[Serializable]
public class StuffSaveData
{
    public string Name;
    public Position Position;
    // public Rotation... add possibility to rotate object in map
}

[Serializable]
public class CreatureSaveData
{
    public string Name;
    public Position Position;
    public List<ItemSaveData> EquipedItems;
    public List<ItemSaveData> CarriedItems;
}

[Serializable]
public class ItemSaveData
{
    public string Name;
    public Position Position;
}

public class SaveAndLoad : Singleton<SaveAndLoad>
{
    
}
