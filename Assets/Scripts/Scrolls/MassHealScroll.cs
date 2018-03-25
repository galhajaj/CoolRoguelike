using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassHealScroll : Scroll
{
    protected override bool activate()
    {
        foreach (Creature member in Party.Instance.Members)
            member.Stats[Stat.HEARTS] = member.Stats[Stat.MAX_HEARTS];
        return true;
    }
}
