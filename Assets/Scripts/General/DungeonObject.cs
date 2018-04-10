using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonObject : MonoBehaviour
{
    public Sprite Image;

    [SerializeField]
    private bool _isBlockPath = false;
    public bool IsBlockPath { get { return _isBlockPath; } }

    [SerializeField]
    private bool _isBlockView = false;
    public bool IsBlockView { get { return _isBlockView; } }

    public Position Position
    {
        get
        {
            if (this.GetComponentInParent<DungeonTile>() == null)
                return Position.NullPosition;
            return this.GetComponentInParent<DungeonTile>().Position;
        }
    }

    public virtual SaveData GetSaveData()
    {
        StuffSaveData saveData = new StuffSaveData();
        saveData.Name = Utils.GetCleanName(gameObject.name);
        saveData.Position = this.Position;
        return saveData;
    }
}
