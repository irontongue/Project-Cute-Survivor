using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLazer : WeaponBase
{
    [SerializeField] float innerRadius;
    [SerializeField] float outerRadius;
    [SerializeField] GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        StartCoroutine(MainLoop());
    }

    IEnumerator MainLoop()
    {
        while (gameManager.isPaused)
            yield return null;

        yield return new WaitForSeconds(weaponStats.attackSpeed);
        while (gameManager.isPaused)
            yield return null;

        Vector2 position = GetPointInDonut(innerRadius, outerRadius);
        if (projectilePool.Count > 0)
        {
            projectilePool[0].transform.position = position;
            projectilePool[0].SetActive(true);
            RemoveFromProjectilePool(projectilePool[0]);
        }
        else
        {
            GameObject proj = Instantiate(projectilePrefab, position, Quaternion.identity, tempObjectsGameObject.transform);
            LazerProjectile lazer = proj.GetComponent<LazerProjectile>();
            lazer.Initilize(position, GetPointInDonut(innerRadius, outerRadius), weaponStats.projectileSpeed, weaponStats.damage, gameManager);
        }
        StartCoroutine(MainLoop());
    }
}
