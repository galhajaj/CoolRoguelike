using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portrait : MonoBehaviour
{
    [SerializeField]
    private Creature _creature = null;
    public Creature Creature { get { return _creature; } }

    [SerializeField]
    private GenericGrid _lifeUnitsGrid = null;
    [SerializeField]
    private GenericGrid _spellUnitsGrid = null;
    [SerializeField]
    private GenericGrid _actionUnitsGrid = null;
    [SerializeField]
    private SpriteRenderer _frameRenderer = null;

    private float _changeAnimationMinTime = 3.0F;
    private float _changeAnimationMaxTime = 10.0F;
    private float _timeToChangeAnimation;

    void Start ()
    {
        _timeToChangeAnimation = Random.Range(_changeAnimationMinTime, _changeAnimationMaxTime);
    }

    void Update()
    {
        // look right/left animation
        if (_timeToChangeAnimation > 0)
        {
            _timeToChangeAnimation -= Time.deltaTime;
        }
        else
        {
            _timeToChangeAnimation = Random.Range(_changeAnimationMinTime, _changeAnimationMaxTime);
            int rand = Random.Range(0, 5);
            string triggerName;
            if (rand == 0)
                triggerName = "LookRightTrigger";
            else if (rand == 1)
                triggerName = "LookLeftTrigger";
            else
                triggerName = "BlinkTrigger";
            GetComponent<Animator>().SetTrigger(triggerName);
        }

        // frame visibility
        bool isSelectedMember = (Party.Instance.SelectedMember == _creature);
        _frameRenderer.enabled = isSelectedMember;

        // units visibility & color of grid units
        _lifeUnitsGrid.UpdateElementsVisibility(_creature.Stats[Stat.HEARTS], _creature.Stats[Stat.MAX_HEARTS], (float)_creature.Stats[Stat.HIT_POINTS] / _creature.Stats[Stat.MAX_HIT_POINTS]);
        _spellUnitsGrid.UpdateElementsVisibility(_creature.Stats[Stat.MANA], _creature.Stats[Stat.MAX_MANA]);
        _actionUnitsGrid.UpdateElementsVisibility(_creature.ActionUnits, Consts.MAX_ACTION_UNITS);
    }

    private void OnMouseDown()
    {
        if (MouseManager.Instance.State != MouseState.DEFAULT)
            return;

        if (_creature.IsActive)
            Party.Instance.SelectedMember = _creature;
    }
}
