using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    public Creature Target;
    public int MinDamage;
    public int MaxDamage;

    void Awake()
    {
        ProjectileManager.Instance.AddFlownProjectile();
    }

    void Start ()
    {
		
	}

    void Update()
    {
        float step = Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, step);

        // hit when arrive to target
        // TODO: add verification of projectile arrival... if speed is zero for instance or I don't know - it can stuck the game
        if (Vector3.Distance(transform.position, Target.transform.position) <= 0.1F)
        {
            // check if hit
            int rand = UnityEngine.Random.Range(0, 101);
            if (rand <= Target.Stats[Stat.ARMOR])
            {
                Debug.Log(this.name + " missed " + Target.name);
                return;
            }

            // hit
            int damage = Random.Range(MinDamage, MaxDamage + 1);
            Target.TakeDamage(damage, DamageType.PHYSICAL);

            Debug.Log(this.name + " hit " + Target.name);

            ProjectileManager.Instance.RemoveFlownProjectile();
            // destroy projectile
            Destroy(this.gameObject);
        }
    }
}
