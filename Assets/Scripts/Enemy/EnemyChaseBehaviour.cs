using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyChaseBehaviour : EnemyInfo
{
    [Header("Ranged Behaviour")]
    [SerializeField] GameObject projectile;
    [SerializeField] float attackSpeed;
    [SerializeField] int projectileDamage = 1;
    [SerializeField] float projectileSpeed;
    [Header("Explode Behaviour")]
    [SerializeField] float explodeRange;
    [SerializeField] ParticleSystem explosionParticleSystem;
    override public void AILoop(Vector3 playerPosition)
    {
        MoveTowardsPlayer(playerPosition);
        switch (enemyBehaviour)
        {
            case EnemyBehaviour.Ranged:
                RangedAttack();
                break;
            case EnemyBehaviour.Explode:
                Explode();
                break;

        }

    }
    
    protected virtual void MoveTowardsPlayer(Vector3 pos)
    {
        Vector3 dir = (player.transform.position - trans.position).normalized;
        dir.z = 0;
        rb.velocity = dir * speed * Time.deltaTime;
    }
    float rangedCD = 0;
    void RangedAttack()
    {
        rangedCD += Time.deltaTime;
        if (rangedCD < attackSpeed)
            return;
        rangedCD = 0;
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (manager.projectilePools[enemyType].Count > 0)
        {
            EnemyProjectile eProj = manager.RemoveFromProjectilePool(enemyType).GetComponent<EnemyProjectile>();
            //manager.projectilePool.RemoveAt(0);
            eProj.transform.position = trans.position;
            eProj.transform.rotation = Quaternion.Euler(0, 0, angle);
            eProj.damage = projectileDamage;
            eProj.speed = projectileSpeed;

        }
        else
        {
            EnemyProjectile eProj = Instantiate(projectile, transform.position, Quaternion.Euler(0,0,angle), manager.tempGameobjectFolder.transform).GetComponent<EnemyProjectile>();
            eProj.damage = projectileDamage;
            eProj.speed = projectileSpeed;
            eProj.enemyType = enemyType;
        }
    }
    float explodeTimer = 0;
    void Explode()
    {
        explodeTimer += Time.deltaTime;
        if (explodeTimer > 0.1f)
        {
            explodeTimer = 0;
            if (Vector2.Distance(player.position, transform.position) < explodeRange)
            {
                Instantiate(explosionParticleSystem, trans.position, Quaternion.identity);
                StartCoroutine(DeathEvent(false));
            }
        }
    }
}
