using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor_MoveState : BossState
{
    [SerializeField] private float speed;
    public bool activateShield;

    public override void Action()
    {
        var direction = (ship.position - transform.position).normalized;
        body.AddForce(direction * speed);

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
