using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEliteBehaviour : EnemyInfo
{
    // Start is called before the first frame update
    [SerializeField] Sprite[] attackFrames;
    [Header("Ranged Behaviour")]
    [SerializeField] GameObject projectile;
    [SerializeField] float rangedAttackCooldown;
    [SerializeField] int projectileDamage = 1;
    [SerializeField] int numberOfProjectiles;
    [SerializeField] int numberOfProjectileWaves;
    [SerializeField] float timeBetweenWaves = 0.25f;
    [SerializeField] float projectileSpreadAngle;
    [SerializeField] float projectileSpeed;
    [Header("Dash Behaviour")]
    [SerializeField] float dashSpeed = 1;
    [SerializeField] float dashRange = 10;
    [SerializeField] float dashCooldown = 5;
    [SerializeField] float dashChargeUpTime = 1;
    float spreadDivided;
    bool rangedAttackLatch = false;
    bool dashLatch = false;


    private void Start()
    {
        isElite = true;
        spreadDivided = projectileSpreadAngle / numberOfProjectiles;
    }
    //void Update()
    //{
    //    if(animate)
    //    {
    //        Animate();
    //    }
    //}
    override public void AILoop(Vector3 playerPosition)
    {
        if (!rangedAttackLatch && !dashLatch)
            MoveTowardsPlayer(playerPosition);

        switch (enemyBehaviour)
        {
            case EnemyBehaviour.Ranged:
                RangedAttack();
                break;
            case EnemyBehaviour.Dash:
                Dash();
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
        if (rangedAttackLatch)
            return;
        rangedCD += Time.deltaTime;
        if (dashLatch)
            return;
        if (rangedCD < rangedAttackCooldown)
            return;
        rb.velocity = Vector2.zero;
        StartCoroutine(FireBullets());

    }
    IEnumerator FireBullets()
    {
        frameTemp = attackFrames;
        currentFrame = 0;
        rangedAttackLatch = true;
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - (spreadDivided * Mathf.Floor(numberOfProjectiles * 0.5f));
        for (int j = 0; j < numberOfProjectileWaves; j++)
        {
            int g = 0;
            for (int i = 0; i < numberOfProjectiles; i++)
            {
                g++;
                if (manager.projectilePool.Count > 0)
                {
                    EnemyProjectile eProj = manager.projectilePool[0].GetComponent<EnemyProjectile>();
                    manager.projectilePool.RemoveAt(0);
                    eProj.transform.position = trans.position;
                    eProj.transform.rotation = Quaternion.Euler(0, 0, angle + (spreadDivided * i));
                    eProj.damage = projectileDamage;
                    eProj.speed = projectileSpeed;
                    eProj.gameObject.SetActive(true);

                }
                else
                {
                    SpawnProjectile(angle + (spreadDivided * i));
                }

            }
            yield return new WaitForSeconds(timeBetweenWaves);
        }
        rangedAttackLatch = false;
        rangedCD = 0;
        frameTemp = frames;
        currentFrame = 0;

    }
    void SpawnProjectile(float angle)
    {
        EnemyProjectile eProj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle), manager.tempGameobjectFolder.transform).GetComponent<EnemyProjectile>();
        eProj.damage = projectileDamage;
        eProj.speed = projectileSpeed;
    }
    float dashCD = 0;
    void Dash()
    {
        if (dashLatch)
            return;
        dashCD += Time.deltaTime;
        if (rangedAttackLatch)
            return;
        if (dashCD < dashCooldown)
            return;
        StartCoroutine(DashAttack());
    }
    IEnumerator DashAttack()
    {
        frameTemp = attackFrames;
        currentFrame = 0;
        dashLatch = true;
        yield return new WaitForSeconds(dashChargeUpTime);
        Vector2 pos = (Vector2)player.position + player.GetComponent<Rigidbody2D>().velocity * (15 / dashSpeed + dashRange);
        while(Vector2.Distance(trans.position, pos) > 0.5f)
        {
            while(gameManger.isPaused)
                yield return null;
            print(Vector2.Distance(trans.position, pos));
            yield return null;
            trans.position = Vector2.MoveTowards(trans.position, pos, dashSpeed * Time.deltaTime);
        }
        dashLatch = false;
        dashCD = 0;
        frameTemp = frames;
        currentFrame = 0;
    }
    override public void DealDamage(int damage)
    {
        if (!active)
            return;

        health -= damage;
        if (health <= 0)
        {
            gameManger.EliteUpgradeEvent();
            StartCoroutine(DeathEvent());
        }
    }

    // Update is called once per frame
}
