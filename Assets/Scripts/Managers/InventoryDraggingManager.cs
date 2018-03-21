using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDraggingManager : MonoBehaviour
{
    private GameObject _draggedObject = null;
    private Vector2 _originalPosition = Vector2.zero;

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
        if (Input.GetMouseButtonDown(0))
        {
            LayerMask layerMask = (1 << LayerMask.NameToLayer("Item"));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.0F, layerMask);

            if (hit.collider != null)
            {
                _draggedObject = hit.collider.gameObject;
                _originalPosition = _draggedObject.transform.position;

                Item item = _draggedObject.GetComponent<Item>();
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
        }
    }

    private void updateMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (_draggedObject != null)
            {
                Item item = _draggedObject.GetComponent<Item>();

                MiniatureManager miniatureManager = Utils.GetObjectUnderCursor<MiniatureManager>("Miniature");
                Pocket pocket = Utils.GetObjectUnderCursor<Pocket>("Pocket");

                // release on miniature
                if (miniatureManager != null)
                {
                    // equip item
                    Party.Instance.SelectedMember.EquipItem(item);
                }
                // release on pocket
                else if (pocket != null)
                {
                    // insert item to pocket
                    Party.Instance.SelectedMember.InsertItemInPocket(item, pocket.gameObject);
                }
                // release on somewhere else
                else
                {
                    // change parent to inventory
                    _draggedObject.transform.parent = (item.Type == ItemType.CURRENCY) ? Bag.Instance.transform : Inventory.Instance.transform;
                }

                _draggedObject = null;
                Bag.Instance.ReorderOutOfBorderItems();
                Inventory.Instance.ReorderOutOfBorderItems();
            }
        }
    }

    private void updateDragging()
    {
        if (_draggedObject == null)
            return;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.0F;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        _draggedObject.transform.position = worldPos;
    }
}
