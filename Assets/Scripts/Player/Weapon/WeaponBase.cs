using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] protected Transform muzzlePosition;
    [SerializeField] protected Weapons weapon;
    [SerializeField] protected GameObject tempObjectsGameObject;

    protected GameObject currentTarget;
    protected EnemyInfo currentTargetInfo = null;
    protected Weapon weaponStats;
    protected GameManager gameManager;
    [SerializeField] protected List<GameObject> projectilePool;


    virtual protected void Start()
    {
        weaponStats = FindObjectOfType<WeaponStats>().GetWeapon(weapon);
        gameManager = FindObjectOfType<GameManager>();
    }
    protected virtual IEnumerator GetClosestTarget()
    {
        currentTarget = null;
        while (currentTarget == null)
        {
            if (gameManager.isPaused)
                yield return null;
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
    protected virtual EnemyInfo[] GetEnemiesInArea(Vector2 center, Vector2 size, float angle)
    {
        EnemyInfo[] infos;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, angle, 1 << 6);
        infos = new EnemyInfo[hits.Length];
        for(int i =0 ; i < infos.Length; i++) 
        {
            infos[i] = hits[i].GetComponent<EnemyInfo>();
        }
        return infos;
    }
    public void AddToProjectilePool(GameObject _gameObject)
    {
        projectilePool.Add(_gameObject);
    }
    public void RemoveFromProjectilePool(GameObject _gameObject)
    {
        projectilePool.Remove(_gameObject);
    }
}
