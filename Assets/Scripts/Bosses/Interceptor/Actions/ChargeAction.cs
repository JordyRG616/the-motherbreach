using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAction : BossAction
{ 
    [SerializeField] private float chargeSpeed;
    private Vector2 targetPosition = Vector2.zero;
    private float counter;
    public bool activateWeaponry;

    public override void Action()
    {
        
    }

    private void Charge()
    {
        if(targetPosition == Vector2.zero)
        {
            targetPosition = ship.position - transform.position;
            targetPosition += Vector2.Perpendicular(targetPosition);
            if(activateWeaponry) actionWeaponry.ForEach(x => x.Shoot());
            // targetPosition -= (Vector2)transform.position;
        }

        body.velocity = Vector2.zero;
        body.AddForce(targetPosition.normalized * chargeSpeed, ForceMode2D.Impulse);

        LookAt(targetPosition);
    }

    private void Retreat(Vector2 direction)
    {
        body.velocity = Vector2.zero;
        var perpendicular = Vector2.Perpendicular(direction);
        perpendicular += perpendicular - direction;
        body.AddForce(perpendicular.normalized * speed, ForceMode2D.Impulse);

        LookAt(ship.position - transform.position);
    }

    public override void DoActionMove()
    {
        Vector2 direction = ship.position - transform.position;

        if(counter < 2) Retreat(direction);
        else Charge();

        counter += Time.fixedDeltaTime;
    }

    public override void EndAction()
    {
        targetPosition = Vector2.zero;
        actionWeaponry.ForEach(x => x.StopShooting());
    }

    public override void StartAction()
    {
        controller.ActivateAnimation("Special");
        counter = 0;   
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, targetPosition);
    }
}
