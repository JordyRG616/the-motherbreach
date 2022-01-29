using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor_Disengage : BossState
{
    [SerializeField] private float speed;

    public override void Action()
    {
        var direction = (Vector2)(transform.position - ship.position).normalized;
        var perpendicular = -Vector2.Perpendicular(direction);

        body.AddForce((direction + perpendicular) * speed);
    }

    public override void EnterState()
    {
        actionController.StopWeapons();
    }

    public override void ExitState()
    {
        
    }
}
