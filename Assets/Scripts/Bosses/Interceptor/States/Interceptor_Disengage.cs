using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor_Disengage : BossState
{
    public bool offensiveRetreat;
    private List<WeaponClass> offensiveRetreatWeapons = new List<WeaponClass>() {WeaponClass.Artillery};

    public override void Action()
    {
        var direction = (Vector2)(transform.position - ship.position).normalized;
        var perpendicular = -Vector2.Perpendicular(direction);

        body.AddForce((direction + perpendicular) * speed *speedModifier);
    }

    public override void EnterState()
    {
       body.velocity = Vector2.zero;
    }

    public override void ExitState()
    {
        
    }
}
