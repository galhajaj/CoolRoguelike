using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingScroll : Scroll
{
    protected override bool activate()
    {
        TargetCreature.Stats[Stat.HEARTS] = TargetCreature.Stats[Stat.MAX_HEARTS];
        return true;
    }
}
