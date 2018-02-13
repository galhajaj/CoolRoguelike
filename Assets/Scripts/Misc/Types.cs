
using System;

[Serializable]
public class StatsDictionary : SerializableDictionary<Stat>
{
    // overloading + operator
    public static StatsDictionary operator +(StatsDictionary d1, StatsDictionary d2)
    {
        foreach (Stat stat in d2.Keys)
            d1[stat] += d2[stat];
        return d1;
    }
}

public enum Direction
{
    NONE,
    UP,
    DOWN,
    NORTH,
    NORTH_WEST,
    NORTH_EAST,
    SOUTH,
    SOUTH_WEST,
    SOUTH_EAST,
    WEST,
    EAST
}

public enum SocketType
{
    NONE,
    HEAD, // for helmet
    NECK, // for necklace
    TORSO, // for armor
    BACK, // for clock
    WRIST, // for bracers
    HAND, // X2
    WAIST, // for belt
    FEET, // for boots
    FINGER, // X10
    RANGED, // bow/crossbow/sling
    AMMO, // quiver for arrows/bolts or stones
    BELT // for potion/throwing knife/scroll/wand
}

public enum ItemType
{
    NONE,
    HELMET,
    ARMOR,
    CLOCK,
    WEAPON,
    SHIELD,
    BELT,
    BOOTS,
    RANGED_WEAPON,
    QUIVER,
    NECKLACE,
    RING,
    POTION
}

public enum ItemCondition
{
    OK,
    SLIGHTLY_DAMAGED,
    DAMAGED,
    BROKEN
}

public enum ItemDurability
{
    FRAGILE,
    BAD,
    NORMAL,
    GOOD,
    EXCELLENT,
    UNBREAKABLE
}

public enum Stat
{
    NULL,
    MAX_HEARTS,
    HEARTS,
    MAX_HIT_POINTS,
    HIT_POINTS,
    MAX_MANA,
    MANA,
    MIN_DAMAGE,
    MAX_DAMAGE,
    MIN_RANGED_DAMAGE,
    MAX_RANGED_DAMAGE,
    ARMOR,
    BELT_SLOTS_NUMBER,
    WEIGHT
}