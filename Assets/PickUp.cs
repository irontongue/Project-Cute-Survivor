using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] int health;
    AudioSource audioSource;
    SpriteRenderer spriteRenderer;
    [SerializeField]ParticleSystem particleSys;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Instantiate(particleSys, transform.position, Quaternion.identity);
            collision.GetComponent<PlayerDamageHandler>().Heal(health);
            audioSource.Play();
            spriteRenderer.enabled = false;
            Destroy(gameObject, audioSource.clip.length);
        }
    }
}
