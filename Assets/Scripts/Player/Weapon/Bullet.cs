using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Weapon weapon;
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        //Destroy(gameObject, 10);
    }
    void Update()
    {
        if (gameManager.isPaused)
            return;
        transform.position += transform.right * weapon.projectileSpeed * Time.deltaTime;
    }

    IEnumerator ResetSelfTimer()
    {
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
            ResetSelf();
        }
	}
}
