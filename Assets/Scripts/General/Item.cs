using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemState
{
    GROUND,
    INVENTORY,
    EQUIPPED
}

public class Item : MonoBehaviour
{
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
        switch (_state)
        {
            case ItemState.GROUND:
                _spriteRenderer.sprite = ResourcesManager.Instance.LootOnGroundSprite;
                break;
            case ItemState.INVENTORY:
                _spriteRenderer.sprite = _originalSprite;
                break;
            case ItemState.EQUIPPED:
                break;
            default:
                break;
        }
    }

    // stats to add when wearing
    public int MinDamage;
    public int MaxDamage;
    public int Armor;
}
