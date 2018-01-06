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
                //Inventory.Instance.PutOffChip(_draggedObject);

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

                if (targetSocketCollider != null) // up on miniature
                {
                    _draggedObject.transform.parent = MiniatureManager.Instance.transform;
                }
                else // up on somewhere else
                {
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
