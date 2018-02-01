using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DungeonSaveData
{
    public string Name;
    public List<AreaSaveData> Dungeons = new List<AreaSaveData>();
}

[Serializable]
public class AreaSaveData
{
    public string Name;
    public List<DungeonObjectSaveData> DungeonObjects = new List<DungeonObjectSaveData>();
    public List<PortalSaveData> Portals = new List<PortalSaveData>();
    public List<CreatureSaveData> Creatures = new List<CreatureSaveData>();
    public List<ItemSaveData> Items = new List<ItemSaveData>();
}

[Serializable]
public class DungeonObjectSaveData
{
    public string Name;
}

[Serializable]
public class PortalSaveData
{
    public string Name;
    public bool Immediate; // true is instant traveling while party on it, otherwise spacebar to climb/went down
    public string TargetArea;
    public string TargetPortalName; // if not immediate
    public Position TargetPosition; // if immediate
}

[Serializable]
public class CreatureSaveData
{
    public string Name;
    public Position Position = new Position();
    public List<ItemSaveData> EquipedItems;
    public List<ItemSaveData> CarriedItems;
}

[Serializable]
public class ItemSaveData
{
    public string Name;
    public Position Position = new Position();
}

public class SaveAndLoad : Singleton<SaveAndLoad>
{
    
}
