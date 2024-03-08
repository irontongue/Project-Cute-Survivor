using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : MonoBehaviour
{
    protected GameObject player;
    protected GameObject gameManager;
    protected Rigidbody2D rb;
    [SerializeField]protected float speed;

    virtual protected void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {
        MoveTowardsPlayer();
    }
    protected virtual void MoveTowardsPlayer()
    {
        Vector3 dir = (player.transform.position - transform.position).normalized;
        dir.z = 0;
        rb.velocity = dir * speed * Time.deltaTime;


    }
}
