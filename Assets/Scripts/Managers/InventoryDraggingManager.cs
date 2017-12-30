using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDraggingManager : MonoBehaviour
{
    private GameObject _draggedObject = null;
    //private GameObject _originSocketObject = null;

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
                //_originSocketObject = _draggedObject.transform.parent.gameObject;
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
            setItemSortingLayer(false);
            _draggedObject = null;

            /*if (_draggedObject != null && _originSocketObject != null)
            {
                // target socket raycast
                LayerMask layerMask = (1 << LayerMask.NameToLayer("SocketsLayer"));
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.0F, layerMask);
                Collider2D targetSocketCollider = hit.collider;

                // check if dragged chip allowed in the target socket & already contained chip can switch places with it
                bool isDraggedChipAllowedInTargetSocket = true;
                if (targetSocketCollider != null)
                {
                    // allow by type
                    Chip.ChipType draggedChipType = _draggedObject.GetComponent<Chip>().Type;
                    string targetSocketTag = targetSocketCollider.gameObject.tag;

                    if (draggedChipType == Chip.ChipType.TURRET && targetSocketTag == "SkillSocketTag")
                        isDraggedChipAllowedInTargetSocket = false;
                    if (draggedChipType != Chip.ChipType.TURRET && targetSocketTag == "TurretSocketTag")
                        isDraggedChipAllowedInTargetSocket = false;

                    // allow by contained chip can switch with it (for example, dragged socketed turret can't switch with 
                    // inventory skill because it'll couse the skill to replace the turret...)
                    if (targetSocketCollider.transform.childCount > 0)
                    {
                        Chip.ChipType containedChipType = targetSocketCollider.transform.GetChild(0).GetComponent<Chip>().Type;
                        string originSocketTag = _originSocketObject.tag;

                        if (containedChipType == Chip.ChipType.TURRET && originSocketTag == "SkillSocketTag")
                            isDraggedChipAllowedInTargetSocket = false;
                        if (containedChipType != Chip.ChipType.TURRET && originSocketTag == "TurretSocketTag")
                            isDraggedChipAllowedInTargetSocket = false;
                    }
                }

                if (targetSocketCollider != null && isDraggedChipAllowedInTargetSocket)
                {
                    // if target socket contains chip, move it to the origin socket of dragged chip (switch places)
                    if (targetSocketCollider.transform.childCount > 0)
                    {
                        GameObject containedChip = targetSocketCollider.transform.GetChild(0).gameObject;
                        Inventory.Instance.PutOffChip(containedChip);
                        Inventory.Instance.PutOnChip(containedChip, _originSocketObject.transform);
                    }

                    // place chip in target socket
                    Inventory.Instance.PutOnChip(_draggedObject, targetSocketCollider.transform);

                    // save to file
                    DataManager.Instance.SaveDataToFile();
                }
                else // if leave drag not on socket - back to origin socket
                {
                    _draggedObject.transform.position = _originSocketObject.transform.position;
                    Inventory.Instance.PutOnChip(_draggedObject, _originSocketObject.transform);
                }

                setItemSortingLayer(false);
                _draggedObject = null;
                //_originSocketObject = null;
            }*/
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
