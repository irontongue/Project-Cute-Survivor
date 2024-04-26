using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] int health;
    AudioSource audioSource;
    SpriteRenderer spriteRenderer;
    [SerializeField]ParticleSystem particleSys;
    [SerializeField] float flashSpeed;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }
    Color flashingColour = Color.white;
    float lerpTime;
    float pingPongTime;
    private void Update()
    {
        pingPongTime += Time.deltaTime * flashSpeed;
        lerpTime = Mathf.PingPong(pingPongTime, 1);
        flashingColour = Color.Lerp(Color.white, new Color(0.5f, 0.5f, 0.5f, 1), lerpTime);
        spriteRenderer.color = flashingColour;
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
