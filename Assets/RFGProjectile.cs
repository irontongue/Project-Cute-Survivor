using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RFGProjectile : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionParticleSystem;
    [SerializeField] GameObject trailObject;
    [SerializeField] float trailLerpSpeed;
    [SerializeField] float trailPosY = 10f;

    public WeaponBase weaponBase;
    public Weapon weaponStats;

    float lerpT;
    Vector2 trailOriginalPos;
    private void OnEnable()
    {
        Vector2 pos = transform.position;
        trailObject.transform.position = new Vector2(pos.x, pos.y + trailPosY);
        trailOriginalPos = trailObject.transform.position;
        trailObject.GetComponent<TrailRenderer>().Clear();
        lerpT = 0;
    }

    void Update()
    {
        if (lerpT >= 1)
        {
            trailObject.GetComponent<TrailRenderer>().Clear();
            return;
        }
        lerpT += trailLerpSpeed * Time.deltaTime;
        trailObject.transform.position = Vector2.Lerp(trailOriginalPos, transform.position, lerpT);
        if(lerpT >= 1)
        {
            Explode();
        }
    }

    void Explode()
    {
        StartCoroutine(Disable());
        explosionParticleSystem.Play();

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, weaponStats.radius, 1 << 6);
        foreach(Collider2D hit in hits) 
        {
            hit.GetComponent<EnemyInfo>().DealDamage(weaponStats.damage);
        }
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(explosionParticleSystem.main.duration);
        weaponBase.AddToProjectilePool(gameObject);
        gameObject.SetActive(false);
    }
}
