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

        // units visibility & color of grid units
        _lifeUnitsGrid.UpdateElementsVisibility(_creature.Hearts, _creature.MaxHearts);
        _spellUnitsGrid.UpdateElementsVisibility(_creature.Mana, _creature.MaxMana);
        _actionUnitsGrid.UpdateElementsVisibility(_creature.ActionUnits, ConstsManager.Instance.MAX_ACTION_UNITS);
    }

    private void OnMouseDown()
    {
        if (_creature.IsActive)
            Party.Instance.SelectedMember = _creature;
    }
}
