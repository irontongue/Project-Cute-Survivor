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
    [Header("SetUp")]
    public Rigidbody2D rb;
    public GameObject gObject;
    public Transform trans;
    public AIManager manager;
    public Transform player;
    public GameManager gameManger;
    public ParticleSystem fire_PE;
    [Header("Animation")]
    public SpriteRenderer spriteRenderer;
    public Sprite[] frames;
    [SerializeField, Tooltip("Input your FPS then / 10")] float framesPerSec;
    public int currentFrame = 0;
    virtual public void AILoop(EnemyBehaviour behaviour, Vector3 playerPosition)
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

    virtual protected IEnumerator DeathEvent()
    {
        yield return new WaitForEndOfFrame();
        while(gameManger.isPaused)
            yield return null;
        fire_PE.Stop();
        onFire = false;
        active = false;
        gameObject.SetActive(false);
        health = maxHealth;
        manager.RemoveActiveEnemy(this);
        gameManger.GiveExp(exp);
        //Play Death sound?
        //Add exp;
    }

    virtual public void ResetVariables()
    {
        active = true;
    }

    void Start()
    {
       
    }
    public float timer = 0;

    public void Animate()
    {
        timer += Time.fixedDeltaTime;
        if (timer < framesPerSec)
            return;
        timer = 0f;

        if (currentFrame >= frames.Length)
            currentFrame = 0;
        spriteRenderer.sprite = frames[currentFrame];
        currentFrame += 1;
    }
}
