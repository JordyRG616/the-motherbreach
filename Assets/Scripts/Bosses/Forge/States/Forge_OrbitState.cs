using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forge_OrbitState : BossState
{
    private int sign = 1;    

    public override void Action()
    {
        var direction = (ship.position - transform.position).normalized;
        var perpendicular = Vector2.Perpendicular(direction) * sign;
        body.AddForce(perpendicular * speed * speedModifier);

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }

    public override void EnterState()
    {
        sign *= -1;
        // body.velocity = Vector2.zero;
    }

    public override void ExitState()
    {
    }
}
