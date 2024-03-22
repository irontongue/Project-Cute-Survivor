using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponTurret : WeaponBase
{ 
    [SerializeField]GameObject projectilePrefab;
    [SerializeField]GameObject tempObjects; //Folder for all temp objects spawned
   
    
    /*
        * Find Enemy
        * shoot bullet in enemy direction
        * muzzle flash
        * audio play
        * Wait for cooldown
        * Repeate
        */
    override protected void Start()
    {
        base.Start();
        StartCoroutine(LookForEnemy());
    }
    
    private void Update()
    {
        if (currentTarget != null)
        { 
            LookAtTarget(); // Optimise to use one angle calc instead of 2
        }
    }
    
    IEnumerator LookForEnemy()
    {
        if(!currentTargetInfo || currentTargetInfo.active == false || Vector2.Distance(currentTargetInfo.trans.position, transform.position) < weaponStats.attackRange)
        {
            yield return StartCoroutine(GetClosestTarget());
        }
        //LookAtTarget();
        yield return new WaitForSeconds(1 / weaponStats.attackSpeed);
        FireWeapon(currentTarget);
    }
    void FireWeapon(GameObject target)
    {
        if (target == null)
            return;
        Vector2 direction = target.transform.position - transform.parent.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject bullet = Instantiate(projectilePrefab,muzzlePosition.transform.position, Quaternion.Euler(new Vector3(0, 0, angle)), tempObjects.transform);
        bullet.GetComponent<Bullet>().weapon = weaponStats;
        StartCoroutine(LookForEnemy());
    }
    void LookAtTarget()
    {
        Vector2 direction = currentTarget.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
