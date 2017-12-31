using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Sprite _originalSprite;

    void Awake()
    {
        _originalSprite = this.GetComponent<SpriteRenderer>().sprite;
    }

    public void SetSpriteToDungeonIcon()
    {
        this.GetComponent<SpriteRenderer>().sprite = ResourcesManager.Instance.LootOnGroundSprite;
    }

    public void SetSpriteToOriginal()
    {
        this.GetComponent<SpriteRenderer>().sprite = _originalSprite;
    }
}
