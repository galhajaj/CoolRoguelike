using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemState
{
    GROUND,
    INVENTORY,
    EQUIPPED,
    ON_BELT
}

public enum ItemType
{
    HELMET,
    MAIL,
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

public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemType _type = ItemType.WEAPON;
    public ItemType Type { get { return _type; } }

    [SerializeField]
    private ItemState _state = ItemState.GROUND;
    public ItemState State
    {
        get { return _state; }
        set
        {
            _state = value;
            setItemSprite();
        }
    }

    private SpriteRenderer _spriteRenderer;
    private Sprite _originalSprite;

    void Awake()
    {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _originalSprite = _spriteRenderer.sprite;
    }

    private void setItemSprite()
    {
        if (_state == ItemState.GROUND)
            _spriteRenderer.sprite = ResourcesManager.Instance.LootOnGroundSprite;
        else
            _spriteRenderer.sprite = _originalSprite;
    }

    // stats to add when wearing
    public int MaxHearts;
    public int MaxMana;
    public int MinDamage;
    public int MaxDamage;
    public int MinRangedDamage;
    public int MaxRangedDamage;
    public int Armor;
}
