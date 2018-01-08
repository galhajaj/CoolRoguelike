using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portrait : MonoBehaviour
{
    [SerializeField]
    private Creature _creature = null;

    [SerializeField]
    private Grid _lifeUnitsGrid = null;
    [SerializeField]
    private Grid _spellUnitsGrid = null;
    [SerializeField]
    private Grid _actionUnitsGrid = null;
    [SerializeField]
    private SpriteRenderer _frameRenderer = null;

    void Start ()
    {

    }

    void Update()
    {
        // frame visibility
        bool isSelectedMember = (Party.Instance.SelectedMember == _creature);
        _frameRenderer.enabled = isSelectedMember;

        // units visibility & color
        updateUnits(_lifeUnitsGrid, _creature.Hearts, _creature.MaxHearts);
        updateUnits(_spellUnitsGrid, _creature.Mana, _creature.MaxMana);
        updateUnits(_actionUnitsGrid, _creature.ActionUnits, ConstsManager.Instance.MAX_ACTION_UNITS);
    }

    private void updateUnits(Grid grid, int number, int maxNumber)
    {
        List<GridElement> elements = grid.Elements;
        for (int i = 0; i < elements.Count; ++i)
        {
            SpriteRenderer currentSpriteRenderer = elements[i].GetComponent<SpriteRenderer>();

            // visibility of over the max
            if (i >= maxNumber)
            {
                currentSpriteRenderer.enabled = false;
                continue;
            }
            else
            {
                currentSpriteRenderer.enabled = true;
            }

            // black over the number
            if (i >= number)
            {
                currentSpriteRenderer.color = Color.black;
                continue;
            }

            // the rest are white
            currentSpriteRenderer.color = Color.white;
        }
    }

    private void OnMouseDown()
    {
        if (_creature.IsActive)
            Party.Instance.SelectedMember = _creature;
    }
}
