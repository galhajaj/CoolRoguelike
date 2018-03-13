
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
    // overloading - operator
    public static StatsDictionary operator -(StatsDictionary d1, StatsDictionary d2)
    {
        foreach (Stat stat in d2.Keys)
            d1[stat] -= d2[stat];
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
    EAST,
    ORIGIN
}

public enum SocketType
{
    NONE,
    HEAD, // for helmet
    NECK, // for necklace
    TORSO, // for armor
    BACK, // for clock
    WRIST, // for bracers
    MAIN_HAND, // weapon/shield
    OFF_HAND, // the same
    WAIST, // for belt
    FEET, // for boots
    FINGER, // X10
    RANGED, // bow/crossbow/sling
    AMMO, // quiver for arrows/bolts or stones
    POCKET // for potion/throwing knife/scroll/wand
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
    POTION,
    SCROLL
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

public enum ItemQuality
{
    LOW,
    NORMAL,
    HIGH
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
    POCKETS,
    WEIGHT,
    EXPERIENCE_POINTS,
    TO_HIT_MELEE,
    TO_HIT_RANGED,
    MAX_RINGS_ALLOWED,
    // attributes
    STRENGTH = 100,
    ENDURANCE,
    AGILITY,
    INTELLECT,
    WILL,
    ACCURACY,
    // skills
    DAGGERS = 200,
    SHORT_SWORDS,
    LONG_SWORDS,
    TWO_HANDED_SWORDS,
    SPERAS,
    HALBERDS,
    CLUBS,
    MACES,
    FLAILS,
    HAND_AXES,
    TWO_HANDED_AXES,
    STAVES,
    WHIPS,
    BOWS,
    CROSSBOWS,
    SMALL_SHIELDS,
    MEDIUM_SHIELDS,
    LARGE_SHIELDS
}

public enum DamageType
{
    PHYSICAL,
    MIND,
    FIRE,
    COLD,
    LIGHTNING,
    POISON
}