﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemState
{
    GROUND,
    INVENTORY,
    EQUIPPED,
    IN_POCKET,
    DRAGGED
}

public class Item : DungeonObject
{
    public StatsDictionary Stats = new StatsDictionary();

    public SocketType SocketType;

    public ItemType Type;

    public int ValueInCopper;

    public GameObject Projectile;

    public int NumberOfTurnsToExpire = -1; // -1 its the default for no expiration date

    [SerializeField]
    private ItemState _state = ItemState.GROUND;
    public ItemState State
    {
        get { return _state; }
        set
        {
            _state = value;
            setItemSpriteAndSortingLayer();
        }
    }

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private Sprite _originalSprite;

    void Awake()
    {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _collider = this.GetComponent<Collider2D>();
        _originalSprite = _spriteRenderer.sprite;
    }

    public void Hide()
    {
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
    }

    public void Show()
    {
        _spriteRenderer.enabled = true;
        _collider.enabled = true;
    }

    private void setItemSpriteAndSortingLayer()
    {
        // TODO: quick fix... should be here? book page sprite will be as original sprite on ground
        if (Type == ItemType.BOOK_PAGE)
        {
            _spriteRenderer.sprite = _originalSprite;
            _spriteRenderer.sortingLayerName = "ItemOnGround";
            _spriteRenderer.sortingOrder = 0;
            return;
        }

        if (_state == ItemState.GROUND)
        {
            _spriteRenderer.sprite = ResourcesManager.Instance.LootOnGroundSprite;
            _spriteRenderer.sortingLayerName = "ItemOnGround";
            _spriteRenderer.sortingOrder = 0;
        }
        else
        {
            _spriteRenderer.sprite = _originalSprite;
            if (_state == ItemState.INVENTORY)
            {
                _spriteRenderer.sortingLayerName = "ItemInInventory";
                _spriteRenderer.sortingOrder = 0;
            }
            else if (_state == ItemState.DRAGGED)
            {
                _spriteRenderer.sortingLayerName = "ItemDragged";
            }
            else if (_state == ItemState.EQUIPPED)
            {
                _spriteRenderer.sortingLayerName = "OverMiniature";
                _spriteRenderer.sortingOrder = 0;
                if (SocketType == SocketType.MAIN_HAND || SocketType == SocketType.OFF_HAND)
                    _spriteRenderer.sortingOrder = 1;
                if (SocketType == SocketType.AMMO || SocketType == SocketType.RANGED)
                    _spriteRenderer.sortingLayerName = "UnderMiniature";
            }
        }
    }

    public override SaveData GetSaveData()
    {
        ItemSaveData saveData = new ItemSaveData();
        saveData.Name = Utils.GetCleanName(gameObject.name);
        saveData.Position = this.Position;
        return saveData;
    }

    // ================================================================================================== //
    public static void AddRandomness(ref ItemSaveData itemSaveData)
    {
        itemSaveData.Durability = getRandomItemDurability();
        itemSaveData.Condition = getRandomItemCondition();
    }
    // ================================================================================================== //
    private static ItemDurability getRandomItemDurability()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand < 2)
            return ItemDurability.UNBREAKABLE; // 2%
        if (rand < 7)
            return ItemDurability.FRAGILE; // 5%
        if (rand < 12)
            return ItemDurability.EXCELLENT; // 5%
        if (rand < 27)
            return ItemDurability.BAD; // 15%
        if (rand < 42)
            return ItemDurability.GOOD; // 15%
        return ItemDurability.NORMAL; // 58%
    }
    // ================================================================================================== //
    private static ItemCondition getRandomItemCondition()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand < 10)
            return ItemCondition.BROKEN; // 10%
        if (rand < 30)
            return ItemCondition.DAMAGED; // 20%
        if (rand < 60)
            return ItemCondition.SLIGHTLY_DAMAGED; // 30%
        return ItemCondition.OK; // 40%
    }
    // ================================================================================================== //
}
