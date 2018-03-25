using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
    private int _flownProjectilesNumber = 0;

    public void AddFlownProjectile()
    {
        _flownProjectilesNumber++;
    }

    public void RemoveFlownProjectile()
    {
        _flownProjectilesNumber--;

        if (_flownProjectilesNumber < 0)
            Debug.LogError("No way... there is a negative number of flown projectiles...");
    }

    public bool IsFlownProjectileExist
    {
        get { return (_flownProjectilesNumber > 0); }
    }

    public void ShootProjectile(GameObject projectilePrefab, Vector3 origin, Creature target, int minDamage, int maxDamage)
    {
        // create projectile
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = origin;
        projectile.transform.right = target.transform.position - transform.position;
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.Target = target;
        projectileScript.MinDamage = minDamage;
        projectileScript.MaxDamage = maxDamage;
    }
}
