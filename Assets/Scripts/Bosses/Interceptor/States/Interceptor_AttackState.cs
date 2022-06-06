using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor_AttackState : BossState
{
    [SerializeField] private List<WeaponClass> weapons;
    private bool slowingDown;


    public override void Action()
    {
        body.velocity = Vector2.zero;
        var direction = (Vector2)(ship.position - transform.position).normalized;
        var _direction = Vector2.Perpendicular(direction);
        body.AddForce(_direction * speed * speedModifier, ForceMode2D.Impulse);
        var angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public override void EnterState()
    {
        // actionController.ActivateWeapons(weapons);
    }

    public override void ExitState()
    {
        actionController.StopWeapons();
    }

}
