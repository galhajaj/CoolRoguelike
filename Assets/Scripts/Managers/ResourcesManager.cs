using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : Singleton<ResourcesManager>
{
    public Sprite LootOnGroundSprite;

    public Sprite FloorDungeonTileSprite;
    public Sprite OutdoorDungeonTileSprite;

    // currency
    public GameObject GoldCoinPrefab;
    public GameObject SilverCoinPrefab;
    public GameObject CopperCoinPrefab;
    public GameObject RubyPrefab;
}
