using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBuzzSaw : WeaponBase
{
    [SerializeField] GameObject projectilePrefab;
    Transform playerT;
    override protected void Start()
    {
        base.Start();
        playerT = FindFirstObjectByType<PlayerController>().transform;
        StartCoroutine(MainLoop());
    }

    IEnumerator MainLoop()
    {
        while (gameManager.isPaused)
            yield return null;

        yield return new WaitForSeconds(1 / weaponStats.attackSpeed);
        while (gameManager.isPaused)
            yield return null;
        PlayAudio();
        if (projectilePool.Count > 0)
        {
            projectilePool[0].transform.position = transform.position;
            projectilePool[0].GetComponent<BuzzSawProjectile>().Initialize();
            projectilePool[0].SetActive(true);
            RemoveFromProjectilePool(projectilePool[0]);
        }
        else
        {
            Quaternion rotation = playerT.rotation;
            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0,0,rotation.eulerAngles.z - 90), tempObjectsGameObject.transform);
            BuzzSawProjectile projectile = proj.GetComponent<BuzzSawProjectile>();
            projectile.weapon = weaponStats;
            projectile.gameManager = gameManager;
            projectile.Initialize();
        }
        StartCoroutine(MainLoop());
    }
}
