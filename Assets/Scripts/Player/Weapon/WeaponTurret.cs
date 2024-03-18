using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponTurret : MonoBehaviour
{
    Weapon weaponStats;
    [SerializeField] Transform muzzlePosition;
    [SerializeField]GameObject projectilePrefab;
    [SerializeField]GameObject tempObjects; //Folder for all temp objects spawned
    GameObject currentTarget;
    EnemyInfo currentTargetInfo = null;
    
    /*
        * Find Enemy
        * shoot bullet in enemy direction
        * muzzle flash
        * audio play
        * Wait for cooldown
        * Repeate
        */
    void Start()
    {
        weaponStats = FindObjectOfType<WeaponStats>().GetWeapon(Weapons.Turret);
        StartCoroutine(LookForEnemy());
    }
    
    private void Update()
    {
        if (currentTarget != null)
        { 
            LookAtTarget(); // Optimise to use one angle calc instead of 2

        }
    }
    GameObject FindClosestTarget()
    {
        GameObject target = null;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, weaponStats.attackRange, 1 << 6);

        if (hits != null) //Erorr check
        {
            GameObject closestTarget = null;
            float closestDistance = Mathf.Infinity;
            foreach (Collider2D hit in hits) //Check all hits for closest object
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = hit.gameObject;
                }
            }
            target = closestTarget;
        }
        return(target);
    }
    IEnumerator LookForEnemy()
    {
        if(!currentTargetInfo || currentTargetInfo.active == false || Vector2.Distance(currentTargetInfo.trans.position, transform.position) < weaponStats.attackRange)
        {
            yield return StartCoroutine(GetEnemies());
        }
        //LookAtTarget();
        yield return new WaitForSeconds(1 / weaponStats.attackSpeed);
        FireWeapon(currentTarget);

    }

    IEnumerator GetEnemies()
    {
        currentTarget = null;
        while (currentTarget == null)
        {
            print("GetENemies");
            currentTarget = FindClosestTarget();
            if (currentTarget)
            { 
                currentTargetInfo = currentTarget.GetComponent<EnemyInfo>();
                break;
            }
            yield return new WaitForSeconds(1 / weaponStats.attackSpeed);
        }
        print(currentTarget);
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
    /*
    void OnDrawGizmosSelected()
    {
        if (currentTarget != null)
        {
            // Calculate direction vector
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;

            // Draw a line from the current object's position to the target position
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, currentTarget.transform.position);

            // Draw a line representing the direction vector
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + direction);

            // Calculate the angle between the direction vector and the x-axis
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Draw a line representing the rotation angle
            Vector3 rotationVector = Quaternion.Euler(0, 0, angle) * Vector3.right;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + rotationVector);
        }
    }
    */


}
