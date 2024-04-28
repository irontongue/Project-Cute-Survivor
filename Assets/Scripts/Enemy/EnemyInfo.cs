using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public EnemyBehaviour enemyBehaviour;
    public EnemyType enemyType;
    public int damage = 1;
    public float speed = 5;
    public int health = 10;
    public int maxHealth = 10;
    public bool active = true;
    public bool onFire = false;
    public int exp = 1;
    //[Header("SetUp")]
    [HideInInspector] public bool isElite = false;
    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector]public GameObject gObject;
    [HideInInspector]public Transform trans;
    [HideInInspector]public AIManager manager;
    [HideInInspector]public Transform player;
    [HideInInspector]public GameManager gameManger;
    [HideInInspector]public ParticleSystem fire_PE;
    [Header("Animation")]
    public bool animate = true;
    public SpriteRenderer spriteRenderer;
    public Sprite[] frames;
    public Sprite[] frameTemp;
    [SerializeField, Tooltip("Input your FPS then / 10")] float framesPerSec;
    public int currentFrame = 0;
    [Header("EliteOnly")]
    [SerializeField] protected AudioClip attackAudioClip;
    protected AudioSource audioSource;
    virtual public void AILoop(Vector3 playerPosition)
    {
        
    }
    public virtual void DamagePerSecond(int damage)
    {
        DealDamage(damage);
    }
    public virtual void SetOnFire()
    {
        onFire = true;
        if(fire_PE != null)
            fire_PE.Play();
    }
    virtual public void DealDamage(int damage)
    {
        if(!active)
            return;

        health -= damage;
        if (health <= 0)
        {
            StartCoroutine(DeathEvent());
        }
    }

    virtual protected IEnumerator DeathEvent(bool giveEXP = true)
    {
        yield return new WaitForEndOfFrame();
        gameObject.SetActive(false);
        while(gameManger.isPaused)
            yield return null;
        if (gameManger.healthPacksAvaliable > 0)
            gameManger.DropHealthPack(trans.position );
        fire_PE.Stop();
        onFire = false;
        active = false;
        health = maxHealth;
        manager.RemoveActiveEnemy(this);
        if(giveEXP)
            gameManger.GiveExp(exp);
        //Play Death sound?
        //Add exp;
    }

    virtual public void ResetVariables()
    {
        active = true;
    }

    public float timer = 0;

    public void Animate()
    {
        if(transform.position.x < player.transform.position.x)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
        if (!animate)
            return;
        timer += Time.fixedDeltaTime;
        if (timer < framesPerSec)
            return;
        timer = 0f;

        if (currentFrame >= frameTemp.Length)
            currentFrame = 0;
        spriteRenderer.sprite = frameTemp[currentFrame];
        currentFrame += 1;
    }
}
