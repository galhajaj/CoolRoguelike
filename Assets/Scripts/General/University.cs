using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class University : MonoBehaviour
{
    [SerializeField]
    private GenericGrid _grid = null;

    [SerializeField]
    private List<Stat> _skills = new List<Stat>();

    // =========================================================================================== //
    void Start ()
    {
        WindowManager.Instance.Event_WindowLoaded += onUniversityWindowLoaded;
        Party.Instance.Event_PartyMemberSelected += onUniversityWindowLoaded;
    }
    // =========================================================================================== //
    void Update ()
    {
        
	}
    // =========================================================================================== //
    public void AddSkill(Stat skillStat)
    {
        _skills.Add(skillStat);
    }
    // =========================================================================================== //
    private void onUniversityWindowLoaded()
    {
        if (!WindowManager.Instance.IsCurrentWindow(Consts.WindowNames.UNIVERSITY))
            return;

        // create the stats to display list, if in village show all, if not - show only the skills with more than 0
        List<Stat> statsToDisplay = new List<Stat>();
        foreach (Stat stat in _skills)
        {
            int playerLevelInCurrentStat = Party.Instance.SelectedMember.Stats[stat];
            if (!Party.Instance.IsInVillage && playerLevelInCurrentStat <= 0)
                continue;
            statsToDisplay.Add(stat);
        }

        // rebuild skills grid
        _grid.Rebuild(statsToDisplay.Count);

        // display skills
        for (int i = 0; i < statsToDisplay.Count; ++i)
        {
            Stat stat = statsToDisplay[i];
            _grid.GetElement(i).GetComponent<SkillButton>().Stat = stat;
            
            // disable clicking if not in town
            if (!Party.Instance.IsInVillage)
                _grid.GetElement(i).GetComponent<SkillButton>().IsActive = false;
        }
    }
    // =========================================================================================== //
}
