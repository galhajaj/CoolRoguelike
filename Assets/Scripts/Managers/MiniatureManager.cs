using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MiniatureManager : Singleton<MiniatureManager>
{
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
}
