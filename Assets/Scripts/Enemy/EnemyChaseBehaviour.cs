using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseBehaviour : EnemyInfo
{
    override public void AILoop(EnemyBehaviour behaviour)
    {
        MoveTowardsPlayer();
    }
    protected virtual void MoveTowardsPlayer()
    {
        Vector3 dir = (player.position - trans.position).normalized;
        dir.z = 0;
        rb.velocity = dir * speed * Time.deltaTime;
    }
}
