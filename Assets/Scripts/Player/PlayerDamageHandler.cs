using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageHandler : MonoBehaviour
{
    public int health = 1;
    public int maxHealth = 1;
    public float invulnerabilityTime = 0.25f;
    [SerializeField] bool invulnerable = false;
    [SerializeField] Image healthBarImage;
    GameManager gameManager;
    public ParticleSystem fx;
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (invulnerable)
        {
            return;
        }
        if(other.transform.tag == "Enemy")
        {
            StartCoroutine(TakeDamage(other.transform.GetComponent<EnemyInfo>().damage));
        }
        else if (other.transform.tag == "Projectile")
        {
            StartCoroutine(TakeDamage(other.GetComponent<EnemyProjectile>().damage));
        }
    }

    public IEnumerator TakeDamage(int damage)
    {
        invulnerable = true;
        ChangeHealth(-damage);                 
        yield return new WaitForSeconds(invulnerabilityTime);
        invulnerable = false;
    }
    public void Heal(int amount) 
    {
        ChangeHealth(amount);
    }
    void ChangeHealth(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        if(health == 0)
        {
            gameManager.DeathEvent();
            fx.Play();
        }
        UpdateUI();
    }
    void UpdateUI()
    {
        healthBarImage.fillAmount = (float)health / (float)maxHealth;
    }
}
