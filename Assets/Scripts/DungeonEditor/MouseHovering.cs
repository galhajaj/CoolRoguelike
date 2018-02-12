using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseHovering : MonoBehaviour
{
    [SerializeField]
    private Text _textHovering = null;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        updateCheckHover();
    }

    private void updateCheckHover()
    {
        LayerMask layerMask = (1 << LayerMask.NameToLayer("DungeonTile"));
        layerMask |= (1 << LayerMask.NameToLayer("SelectorTile"));

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.0F, layerMask);
        if (hit.collider == null)
        {
            _textHovering.text = "";
            return;
        }

        _textHovering.text = "";
        foreach (Transform transform in hit.collider.transform)
        {
            _textHovering.text += "[" + Utils.GetCleanName(transform.name) + "]";
        }
    }
}
