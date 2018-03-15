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
        checkClickOnPocket();
    }
    // ====================================================================================================== //
    private void checkClickOnPocket()
    {
        if (WindowManager.Instance.IsCurrentWindow(Consts.WindowNames.DUNGEON) && Input.GetMouseButtonDown(0))
        {
            Pocket pocket = Utils.GetObjectUnderCursor<Pocket>("Pocket");
            if (pocket != null)
            {
                if (pocket.Index < Party.Instance.SelectedMember.Stats[Stat.POCKETS])
                {
                    Debug.Log("Pocket selected!");
                    Party.Instance.SelectedMember.SelectedPocketIndex = pocket.Index;
                }
            }
        }
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

        // hide (deactivate) all items on pockets
        foreach (Transform beltSlotTransform in this.transform)
            foreach (Transform itemTransform in beltSlotTransform)
                itemTransform.gameObject.SetActive(false);

        // show (activate) items in selected member pockets
        List<Item> selectedMemberBeltItems = Party.Instance.SelectedMember.ItemsInPockets.Values.ToList();
        foreach (Item item in selectedMemberBeltItems)
        {
            item.gameObject.SetActive(true);

            // remove items from pockets when are placed in not exist pocket
            if (item.transform.parent.GetComponent<Pocket>().Index >= Party.Instance.SelectedMember.Stats[Stat.POCKETS])
                Party.Instance.SelectedMember.RemoveItemFromPockets(item);
        }
            

        // mark selected pocket
        foreach (GridElement pocket in _pocketGrid.Elements)
        {
            pocket.GetComponent<SpriteRenderer>().color =
                (pocket.Index == Party.Instance.SelectedMember.SelectedPocketIndex) ? Color.red : Color.white;
        }
    }
    // ====================================================================================================== //
}
