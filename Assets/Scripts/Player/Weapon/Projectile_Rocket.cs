using System.Collections;
using UnityEditor;
using UnityEngine;

public class Projectile_Rocket : MonoBehaviour
{
    public Weapon weapon;
    public Vector2 targetPosition;
    public Vector2 direction;
    public ParticleSystem explosionParticleSystem;
    Vector2 startLocation;
    public GameManager gameManager;
    public WeaponBase weaponBase;
    bool active = true;
    AudioSource audioSource;
    // MAKE THE PARTICLE EFFECT MIMIC THE RADIUS OF THE ATTACK
    public void InitiliseRocket()
    {
        audioSource = GetComponent<AudioSource>();
        startLocation = transform.position;
        gameObject.SetActive(true);
        Vector2 angle = targetPosition - (Vector2)transform.position;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(angle.x, angle.y) * Mathf.Rad2Deg));
    }
    void Update()
    {
        if (gameManager.isPaused || !active)
            return;
        if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
        {
            Explode();
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * weapon.projectileSpeed);
    }
void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(targetPosition, weapon.radius, 1 << 6);
        foreach(Collider2D hit in hits ) 
        {
            EnemyInfo info = hit.GetComponent<EnemyInfo>();
            if (info != null)
            {
                info.DealDamage(weapon.damage);
            }
        }

        StartCoroutine(WaitForParticleEffect());
    }
    IEnumerator WaitForParticleEffect()
    {
        audioSource.Play();
        active = false;
        explosionParticleSystem.Play();
        yield return new WaitForSeconds(explosionParticleSystem.main.duration);
        gameObject.SetActive(false);
        weaponBase.AddToProjectilePool(gameObject);
        active = true;
    }
}
