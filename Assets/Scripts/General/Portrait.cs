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

    void Start ()
    {
        _creature.Event_StatsUpdated += creature_Event_StatsUpdated;
    }

    void Update()
    {

    }

    private void creature_Event_StatsUpdated()
    {
        _lifeUnitsGrid.Rebuild(1, _creature.MaxHearts);
        _spellUnitsGrid.Rebuild(1, _creature.MaxMana);
        _actionUnitsGrid.Rebuild(ConstsManager.Instance.MAX_ACTION_UNITS);

        updateUnitsColor(_lifeUnitsGrid, _creature.Hearts, _creature.MaxHearts);
        updateUnitsColor(_spellUnitsGrid, _creature.Mana, _creature.MaxMana);
        updateUnitsColor(_actionUnitsGrid, _creature.ActionUnits, ConstsManager.Instance.MAX_ACTION_UNITS);
    }

    private void updateUnitsColor(Grid grid, int number, int maxNumber)
    {
        List<GridElement> elements = grid.Elements;
        for (int i = 0; i < elements.Count; ++i)
        {
            elements[i].GetComponent<SpriteRenderer>().color = Color.white;
            if (i >= number)
            {
                elements[i].GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }
}
