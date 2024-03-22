using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseBehaviour : EnemyInfo
{
    override public void AILoop(EnemyBehaviour behaviour, Vector3 playerPosition)
    {
        MoveTowardsPlayer(playerPosition);
    }
    protected virtual void MoveTowardsPlayer(Vector3 pos)
    {
        Vector3 dir = (player.transform.position - trans.position).normalized;
        dir.z = 0;
        rb.velocity = dir * speed * Time.deltaTime;
    }
    private void Update()
    {
        
    }
}
