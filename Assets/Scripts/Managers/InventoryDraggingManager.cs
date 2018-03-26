using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDraggingManager : MonoBehaviour
{
    private Item _draggedItem = null;

    void Start ()
    {
		
	}
	
	void Update ()
    {
        if (!WindowManager.Instance.IsCurrentWindow(Consts.WindowNames.INVENTORY) && 
            !WindowManager.Instance.IsCurrentWindow(Consts.WindowNames.BAG))
            return;

        updataMouseDown();
        updateMouseUp();
        updateDragging();
    }

    private void updataMouseDown()
    {
        if (!Input.GetMouseButtonDown(0))
            return;
        
        Item item = Utils.GetObjectUnderCursor<Item>("Item");
        if (item == null)
            return;

        // disallow moving miniature or pocket items while in bag window
        if (WindowManager.Instance.IsCurrentWindow(Consts.WindowNames.BAG) && 
            (item.State == ItemState.EQUIPPED || item.State == ItemState.IN_POCKET))
            return;

        _draggedItem = item;

        if (item.State == ItemState.EQUIPPED)
        {
            // remove item if equipped
            Party.Instance.SelectedMember.RemoveItem(item);
        }
        else if (item.State == ItemState.IN_POCKET)
        {
            // remove item if equipped
            Party.Instance.SelectedMember.RemoveItemFromPockets(item);
        }

        item.State = ItemState.DRAGGED;

        // save to file
        //DataManager.Instance.SaveDataToFile();
    }

    private void updateMouseUp()
    {
        if (!Input.GetMouseButtonUp(0))
            return;

        if (_draggedItem == null)
            return;

        MiniatureManager miniatureManager = Utils.GetObjectUnderCursor<MiniatureManager>("Miniature");
        Pocket pocket = Utils.GetObjectUnderCursor<Pocket>("Pocket");

        // release on miniature
        if (miniatureManager != null)
        {
            // equip item
            Party.Instance.SelectedMember.EquipItem(_draggedItem);
        }
        // release on pocket
        else if (pocket != null)
        {
            // insert item to pocket
            Party.Instance.SelectedMember.InsertItemInPocket(_draggedItem, pocket.gameObject);
        }
        // release on somewhere else
        else
        {
            // change parent to inventory
            _draggedItem.transform.parent = (_draggedItem.Type == ItemType.CURRENCY) ? Bag.Instance.transform : Inventory.Instance.transform;
        }

        _draggedItem = null;
        Bag.Instance.ReorderOutOfBorderItems();
        Inventory.Instance.ReorderOutOfBorderItems();
    }

    private void updateDragging()
    {
        if (_draggedItem == null)
            return;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.0F;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        _draggedItem.transform.position = worldPos;
    }
}
