using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFireDamage : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
           EnemyInfo info = collision.GetComponent<EnemyInfo>();
            info.DealDamage(damage);
            info.SetOnFire();
        }
    }
}
