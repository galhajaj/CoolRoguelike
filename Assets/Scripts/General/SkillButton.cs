using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : GameButton
{
    private Stat _stat;
    public Stat Stat
    {
        get { return _stat; }
        set
        {
            _stat = value;
            this.refresh();
        }
    }
    //public SkillTitle Title;

    // =========================================================================================== //
    protected override void afterClicked()
    {
        int statLevel = Party.Instance.SelectedMember.Stats[Stat];
        int experienceCostForNextLevel = statLevel + 1;
        int copperCostForNextTitle = 100 * (1 + (statLevel / Consts.LEVELS_PER_SKILL_TITLE));

        // if divided in LEVELS_PER_SKILL_TITLE need to pay gold
        if (statLevel % Consts.LEVELS_PER_SKILL_TITLE == 0)
        {
            if (Bag.Instance.Copper >= copperCostForNextTitle)
            {
                Bag.Instance.RemoveCopper(copperCostForNextTitle);
                Party.Instance.SelectedMember.Stats[Stat]++;
            }
        }
        // otherwise , pay with experience
        else
        {
            if (Party.Instance.SelectedMember.Stats[Stat.EXPERIENCE_POINTS] >= experienceCostForNextLevel)
            {
                Party.Instance.SelectedMember.Stats[Stat.EXPERIENCE_POINTS] -= experienceCostForNextLevel;
                Party.Instance.SelectedMember.Stats[Stat]++;
            }
        }

        this.refresh();
    }
    // =========================================================================================== //
    public void refresh()
    {
        // set skill sprite
        Sprite skillSprite = Resources.Load<Sprite>("Skills/" + Stat);
        this.transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = skillSprite;
        // set skill level
        int playerLevelInCurrentStat = Party.Instance.SelectedMember.Stats[Stat];
        this.transform.Find("Level").GetComponent<TextMesh>().text = playerLevelInCurrentStat.ToString();
    }
    // =========================================================================================== //
}
