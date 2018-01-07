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
        if (WindowManager.Instance.CurrentWindowName != "Inventory")
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
                setItemSortingLayer(true);
                _originalPosition = _draggedObject.transform.position;

                Item item = _draggedObject.GetComponent<Item>();
                if (item.State == ItemState.EQUIPPED)
                {
                    // remove item if equipped
                    Party.Instance.GetActiveMember().RemoveItem(item);
                }

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
                LayerMask layerMask = (1 << LayerMask.NameToLayer("Miniature"));
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.0F, layerMask);
                Collider2D targetSocketCollider = hit.collider;

                // release on miniature
                if (targetSocketCollider != null) 
                {
                    // equip item
                    Item item = _draggedObject.GetComponent<Item>();
                    Party.Instance.GetActiveMember().EquipItem(item);
                }
                // release on somewhere else
                else
                {
                    // change parent to inventory
                    _draggedObject.transform.parent = Inventory.Instance.transform;
                }

                setItemSortingLayer(false);
                _draggedObject = null;
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

    private void setItemSortingLayer(bool isDragged)
    {
        if (_draggedObject == null)
            return;

        string sortingLayerName = isDragged ? "ItemDraggedInInventory" : "ItemInInventory";
        _draggedObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
    }
}
