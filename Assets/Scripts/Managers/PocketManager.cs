using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PocketManager : Singleton<PocketManager>
{
    [SerializeField]
    private Grid _beltGrid = null;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        // units visibility & color of grid units
        _beltGrid.UpdateElementsVisibility(Party.Instance.SelectedMember.Stats[Stat.POCKETS]);

        // hide/show items on belt by the selected member
        // TODO: improve performance?
        List<Item> selectedMemberBeltItems = Party.Instance.SelectedMember.ItemsOnBelt.Values.ToList();
        foreach (Transform beltSlotTransform in this.transform)
        {
            foreach (Transform itemTransform in beltSlotTransform)
            {
                Item item = itemTransform.GetComponent<Item>();
                itemTransform.gameObject.SetActive(selectedMemberBeltItems.Contains(item));
            }
        }
    }

    public void AddItem(Item item, GameObject pocket)
    {
        item.transform.position = pocket.transform.position;
        item.transform.parent = pocket.transform;
        item.State = ItemState.ON_BELT;
    }
}
