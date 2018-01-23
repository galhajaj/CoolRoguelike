using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AreaData
{
    public string name;
    public List<DungeonData> dungeons = new List<DungeonData>();
}

[Serializable]
public class DungeonData
{
    public string name;
    public List<CreatureData> creatures = new List<CreatureData>();
}

[Serializable]
public class CreatureData
{
    public string name;
    public Position position = new Position();
    public List<ItemData> EquipedItems;
    public List<ItemData> CarriedItems;
}

[Serializable]
public class ItemData
{
    public string name;
    public Position position = new Position();
}

public class SaveAndLoad : Singleton<SaveAndLoad>
{
    
}
