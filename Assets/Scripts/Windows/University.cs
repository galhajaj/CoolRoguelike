using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class University : Singleton<University>
{
    [SerializeField]
    private GenericGrid _grid = null;
    [SerializeField]
    private TextMesh _textMeshExperience = null;

    [SerializeField]
    private List<Stat> _skills = new List<Stat>();
    public List<Stat> Skills { get { return _skills; } }

    // =========================================================================================== //
    void Start ()
    {
        WindowManager.Instance.Event_WindowLoaded += onUniversityWindowLoaded;
        Party.Instance.Event_PartyMemberSelected += onUniversityWindowLoaded;
    }
    // =========================================================================================== //
    void Update ()
    {
        _textMeshExperience.text = "Experience Points: " + Party.Instance.SelectedMember.Stats[Stat.EXPERIENCE_POINTS].ToString();
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

        displaySkills();
    }
    // =========================================================================================== //
    private void displaySkills()
    {
        // rebuild skills grid
        _grid.Rebuild(_skills.Count);

        // display skills
        for (int i = 0; i < _skills.Count; ++i)
        {
            Stat stat = _skills[i];
            _grid.GetElement(i).GetComponent<SkillButton>().Stat = stat;
        }
    }
    // =========================================================================================== //
}
