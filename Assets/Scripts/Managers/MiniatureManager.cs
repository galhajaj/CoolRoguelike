using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MiniatureManager : Singleton<MiniatureManager>
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer = null;

    void Update()
    {
        // TODO: improve performance?
        List<Item> selectedMemberItems = Party.Instance.SelectedMember.EquippedItems.Values.ToList();

        foreach (Transform itemTransform in this.transform)
        {
            Item item = itemTransform.GetComponent<Item>();
            itemTransform.gameObject.SetActive(selectedMemberItems.Contains(item));
        }
    }

    public void AddItem(Item item)
    {
        //item.transform.position = this.transform.position;
        item.transform.parent = this.transform;
        item.State = ItemState.EQUIPPED;
    }

    public void SetMiniatureImage(Sprite image)
    {
        _spriteRenderer.sprite = image;
    }

    public void ShowCreature(Creature creature)
    {
        // TODO: implement
        // show it's image (use the function this.ShowImage(...))
        // show it's equipped items (in inventory can hold them and return them to inventory)
    }

    public void ShowImage(Sprite image)
    {
        // TODO: implement
    }
}
