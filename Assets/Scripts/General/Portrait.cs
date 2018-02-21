using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portrait : MonoBehaviour
{
    [SerializeField]
    private Creature _creature = null;
    public Creature Creature { get { return _creature; } }

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
        _lifeUnitsGrid.UpdateElementsVisibility(_creature.Stats[Stat.HEARTS], _creature.Stats[Stat.MAX_HEARTS]);
        _spellUnitsGrid.UpdateElementsVisibility(_creature.Stats[Stat.MANA], _creature.Stats[Stat.MAX_MANA]);
        _actionUnitsGrid.UpdateElementsVisibility(_creature.ActionUnits, Consts.MAX_ACTION_UNITS);
    }

    private void OnMouseDown()
    {
        if (_creature.IsActive)
            Party.Instance.SelectedMember = _creature;
    }
}
