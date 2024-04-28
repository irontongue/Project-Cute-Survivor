using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuzzSawProjectile : MonoBehaviour
{
    public Weapon weapon;
    public GameManager gameManager;
    int bounces;
    bool bouncing;
    Transform targetT;

    public void Initialize()
    {
        bounces = weapon.projectileBounces;
        StartCoroutine(ResetSelfTimer());
    }
    void Update()
    {
        if (gameManager.isPaused)
            return;
            transform.position += transform.right * weapon.projectileSpeed * Time.deltaTime;
        
    }

    IEnumerator ResetSelfTimer()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(weapon.projectileLifetime);
        ResetSelf();
    }
    private void ResetSelf()
    {
        StopCoroutine(ResetSelfTimer());
        gameObject.SetActive(false);
        //Add to pool

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyInfo info;
        if (info = other.gameObject.GetComponent<EnemyInfo>())
        {
            info.DealDamage(weapon.damage);
            StopCoroutine(ResetSelfTimer());
            if(bounces > 0)
            {
                Vector2 direction = transform.position - other.transform.position.normalized;
                float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle + -90);
                bounces--;
            }
            else
            {
                ResetSelf();
            }
        }
    }
}
