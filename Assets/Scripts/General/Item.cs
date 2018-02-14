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

    public override SaveData GetSaveData()
    {
        ItemSaveData saveData = new ItemSaveData();
        saveData.Name = Utils.GetCleanName(gameObject.name);
        return saveData;
    }
}
