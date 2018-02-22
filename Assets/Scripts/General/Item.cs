using System;
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

public class Item : DungeonObject
{
    public StatsDictionary Stats = new StatsDictionary();

    public SocketType SocketType;
    public ItemType Type;

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

    private void setItemSprite()
    {
        if (_state == ItemState.GROUND)
            _spriteRenderer.sprite = ResourcesManager.Instance.LootOnGroundSprite;
        else
            _spriteRenderer.sprite = _originalSprite;
    }

    public override SaveData GetSaveData()
    {
        ItemSaveData saveData = new ItemSaveData();
        saveData.Name = Utils.GetCleanName(gameObject.name);
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
