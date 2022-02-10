using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor_MoveState : BossState
{
    public bool activateShield;

    public override void Action()
    {
        // body.velocity = Vector2.zero;
        var direction = (ship.position - transform.position).normalized;
        body.AddForce(direction * speed * speedModifier);

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {
        
    }
}
