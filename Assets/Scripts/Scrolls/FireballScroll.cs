using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScroll : Scroll
{
    public int MinDamage = 10;
    public int MaxDamage = 30;

    protected override bool activate()
    {
        // shoot fireball
        ProjectileManager.Instance.ShootProjectile(this.Projectile,
            Party.Instance.transform.position, TargetCreature, MinDamage, MaxDamage);

        return true;
    }
}
