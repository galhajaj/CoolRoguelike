using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PocketManager : Singleton<PocketManager>
{
    [SerializeField]
    private GenericGrid _pocketGrid = null;

    // ====================================================================================================== //
    void Start ()
    {
        
    }
    // ====================================================================================================== //
    void Update()
    {
        updatePockets();
    }
    // ====================================================================================================== //
    public void AddItem(Item item, GameObject pocket)
    {
        item.transform.position = pocket.transform.position;
        item.transform.parent = pocket.transform;
        item.State = ItemState.IN_POCKET;
    }
    // ====================================================================================================== //
    // update pockets for the selected party member
    private void updatePockets()
    {
        // units visibility & color of grid units
        _pocketGrid.UpdateElementsVisibility(Party.Instance.SelectedMember.Stats[Stat.POCKETS]);

        // move over all members, deactivate pocket items of the non-selected members & activate the selected member pocket items
        foreach (Creature member in Party.Instance.Members)
        {
            List<Item> currentMemberBeltItems = member.ItemsInPockets.Values.ToList();
            foreach (Item item in currentMemberBeltItems)
            {
                // set active if selected member, inactive otherwise
                item.gameObject.SetActive(Party.Instance.SelectedMember == member);

                // remove items from pockets when are placed in not exist pocket
                if (item.transform.parent.GetComponent<Pocket>().Index >= member.Stats[Stat.POCKETS])
                    member.RemoveItemFromPockets(item);
            }
        }
            

        // mark selected pocket
        /*foreach (GridElement pocket in _pocketGrid.Elements)
        {
            pocket.transform.Find("Frame").GetComponent<SpriteRenderer>().enabled =
                (pocket.Index == Party.Instance.SelectedMember.SelectedPocketIndex && // the selected
                pocket.Index < Party.Instance.SelectedMember.Stats[Stat.POCKETS])  // in exist pockets
                ? true : false;
        }*/
    }
    // ====================================================================================================== //
}
