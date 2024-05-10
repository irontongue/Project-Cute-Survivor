using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRFG : WeaponBase
{
    // Start is called before the first frame update
    [SerializeField] float innerRadius = 5f;
    [SerializeField] float outerRadius = 10f;
    [SerializeField] GameObject projectilePrefab;
    override protected void Start()
    {
        base.Start();
        StartCoroutine(MainLoop());   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator MainLoop()
    {
        while (gameManager.isPaused)
            yield return null;

        yield return new WaitForSeconds(1 / weaponStats.attackSpeed);
        while (gameManager.isPaused)
            yield return null;
        PlayAudio();
        Vector2 position = GetPointInDonut(innerRadius, outerRadius);//
        if(projectilePool.Count > 0)
        {
            projectilePool[0].transform.position = position;
            projectilePool[0].SetActive(true);
            RemoveFromProjectilePool(projectilePool[0]);
        }
        else
        {
            GameObject proj = Instantiate(projectilePrefab, position, Quaternion.identity, tempObjectsGameObject.transform);
            RFGProjectile RFG = proj.GetComponent<RFGProjectile>();
            RFG.weaponBase = this;
            RFG.weaponStats = weaponStats;
        }
        StartCoroutine(MainLoop());
    }

}