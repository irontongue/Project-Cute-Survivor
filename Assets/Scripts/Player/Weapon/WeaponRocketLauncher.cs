using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRocketLauncher : WeaponBase
{
    Vector2 attackRange;
    Vector2 targetPosition;
    [SerializeField] GameObject rocketProjectilePrefab;
    protected override void Start()
    {
        base.Start();
        StartCoroutine(WeaponLoop());
    }

    IEnumerator WeaponLoop()
    {
        yield return new WaitForSeconds(1 / weaponStats.attackSpeed);
        while(gameManager.isPaused)
                yield return null;
        if (GetRandomTransformPosition() == false)
        {
            StartCoroutine(WeaponLoop());
            yield break;
        }
        FireWeapon();
        StartCoroutine(WeaponLoop());
    }

    bool GetRandomTransformPosition() 
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, weaponStats.attackRange, 1 << 6);
        if (hits.Length == 0)
        {
            targetPosition = Vector2.zero;
            return false;
        }
        int randomNumber = Random.Range(0, hits.Length);
        targetPosition = hits[randomNumber].transform.position;
        return true;
    }

    void FireWeapon()
    {
        Projectile_Rocket rocket;
        PlayAudio();
        if (projectilePool.Count > 0)
        {
            rocket = projectilePool[0].GetComponent<Projectile_Rocket>();
            rocket.targetPosition = targetPosition;
            rocket.transform.position = muzzlePosition.position;
            rocket.InitiliseRocket();
            RemoveFromProjectilePool(rocket.gameObject);
            return;
        }

        rocket = Instantiate(rocketProjectilePrefab, muzzlePosition.position, Quaternion.identity, tempObjectsGameObject.transform).GetComponent<Projectile_Rocket>();
        rocket.targetPosition = targetPosition;
        rocket.weapon = weaponStats;
        rocket.gameManager = gameManager;
        rocket.weaponBase = this;
        rocket.InitiliseRocket();
    }

}
