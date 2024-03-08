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
    [Header("SetUp")]
    public Rigidbody2D rb;
    public GameObject gObject;
    public Transform trans;
    public AIManager manager;
    public Transform player;

    virtual public void AILoop(EnemyBehaviour behaviour)
    {
       switch (behaviour) 
        {
            case EnemyBehaviour.Chase:
                break;
            case EnemyBehaviour.Ranged:
                break;
        }
    }
    virtual public void DealDamage(int damage)
    {
        if(!active)
            return;

        health -= damage;
        if (health <= 0)
        {
            DeathEvent();
        }
    }

    virtual protected void DeathEvent()
    {
        active = false;
        gameObject.SetActive(false);
        health = maxHealth;
        manager.RemoveActiveEnemy(this);
        //Play Death sound
    }

    virtual public void ResetVariables()
    {
        active = true;
    }
}
