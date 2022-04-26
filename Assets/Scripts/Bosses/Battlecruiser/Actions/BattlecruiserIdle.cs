using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlecruiserIdle : BossIdle
{
    [SerializeField] [Range(-5, 5)] private float eccentricity;
    [SerializeField] [Range(-1, 1)] private float forceDirection;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    public int sign {get; private set;} = 1;
    private Vector2 direction;

    public override void IdleMove()
    {
        body.velocity = Vector2.zero;

        var distance = Vector2.Distance(ship.position, transform.position);
        
        direction = (ship.position - transform.position) * forceDirection;

        direction += Vector2.Perpendicular(direction) * eccentricity * sign;

        body.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
        LookAt(direction);
    }

    public void CHANGEDIRECTION()
    {
        sign *= -1;
    }
}
