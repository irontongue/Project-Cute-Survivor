using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] protected Transform muzzlePosition;
    [SerializeField] protected Weapons weapon;
    protected GameObject currentTarget;
    protected EnemyInfo currentTargetInfo = null;
    protected Weapon weaponStats;


    virtual protected void Start()
    {
        weaponStats = FindObjectOfType<WeaponStats>().GetWeapon(weapon);
    }
    protected virtual IEnumerator GetClosestTarget()
    {
        currentTarget = null;
        while (currentTarget == null)
        {
            currentTarget = FindClosestTarget();
            if (currentTarget != null)
            {
                currentTargetInfo = currentTarget.GetComponent<EnemyInfo>();
                break;
            }
            yield return new WaitForSeconds(1 / weaponStats.attackSpeed);
        }
    }
    protected virtual GameObject FindClosestTarget()
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
        return (target);
    }
    float _angle;
    Vector2 _size;
    protected virtual EnemyInfo[] GetEnemiesInArea(Vector2 center, Vector2 size, float angle)
    {
        _angle = angle;
        _size = size;
        EnemyInfo[] infos;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, angle, 1 << 6);
        infos = new EnemyInfo[hits.Length];
        for(int i =0 ; i < infos.Length; i++) 
        {
            infos[i] = hits[i].GetComponent<EnemyInfo>();
        }
        return infos;
    }
}
