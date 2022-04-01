using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeIdle : BossIdle
{
    [SerializeField] private float distanceToOrbit;
    [SerializeField] private float distanceToFlee;
    private Vector2 direction;

    public override void IdleMove()
    {
        direction = (ship.position - transform.position).normalized;
        var distance = Vector2.Distance(ship.position, transform.position);

        if(distance <= distanceToFlee) Flee();
        else if(distance <= distanceToOrbit) Orbit();
        else Chase();

        LookAt(direction);
    }

    private void Flee()
    {
        body.velocity = Vector2.zero;
        var perpendicular = Vector2.Perpendicular(direction) * Mathf.Sin(Time.timeSinceLevelLoad);
        var _direction = perpendicular + direction;
        body.AddForce(- _direction.normalized * speed, ForceMode2D.Impulse);
    }

    private void Chase()
    {
        body.velocity = Vector2.zero;
        var perpendicular = Vector2.Perpendicular(direction) * Mathf.Sin(Time.timeSinceLevelLoad);
        var _direction = perpendicular + direction;
        body.AddForce(_direction.normalized * speed, ForceMode2D.Impulse);
    }

    private void Orbit()
    {
        body.velocity = Vector2.zero;
        var direction = (ship.position - transform.position).normalized;
        var perpendicular = Vector2.Perpendicular(direction);
        body.AddForce(perpendicular * speed, ForceMode2D.Impulse);

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }
}
