
using System;

[Serializable]
public class StatsDictionary : SerializableDictionary<Stat> { }

public enum SocketType
{
    NONE,
    HEAD,
    NECK, // for necklace
    TORSO,
    BACK, // for clock
    HAND, // X2
    WAIST, // for belt
    FEET,
    FINGER, // X10
    RANGED,
    AMMO,
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