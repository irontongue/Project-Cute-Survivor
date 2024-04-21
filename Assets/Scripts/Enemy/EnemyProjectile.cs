using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage;
    public float speed;
    [SerializeField] float projectileLifeTime;
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
        transform.position += transform.right * speed * Time.deltaTime;
    }

    IEnumerator ResetSelfTimer()
    {
        yield return new WaitForSeconds(projectileLifeTime);
        StartCoroutine(ResetSelf());
    }
    private IEnumerator ResetSelf()
    {
        yield return new WaitForSeconds(0.05f);
        StopCoroutine(ResetSelfTimer());
        gameObject.SetActive(false);
        gameManager.aiManager.projectilePool.Add(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(ResetSelf());
        }
    }
}
