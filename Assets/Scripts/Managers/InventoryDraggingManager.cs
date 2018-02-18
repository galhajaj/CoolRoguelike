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
        if (!WindowManager.Instance.IsCurrentWindow(Consts.INVENTORY))
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
                    Party.Instance.SelectedMember.RemoveItem(item);
                }
                else if (item.State == ItemState.ON_BELT)
                {
                    // remove item if equipped
                    Party.Instance.SelectedMember.RemoveItemFromBelt(item);
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
                Item item = _draggedObject.GetComponent<Item>();
                Collider2D miniatureCollider = GetColliderUnderCursor("Miniature");
                Collider2D beltCollider = GetColliderUnderCursor("Belt");

                // release on miniature
                if (miniatureCollider != null)
                {
                    // equip item
                    Party.Instance.SelectedMember.EquipItem(item);
                }
                // release on belt
                else if (beltCollider != null)
                {
                    // insert item to pocket
                    Party.Instance.SelectedMember.InsertItemToBelt(item, beltCollider.gameObject);
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

    private Collider2D GetColliderUnderCursor(string layer)
    {
        int layerIndex = LayerMask.NameToLayer(layer);
        if (layerIndex == -1)
            Debug.LogError(layer + " layer not found!");
        LayerMask layerMask = (1 << layerIndex);
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.0F, layerMask);
        return hit.collider;
    }
}
