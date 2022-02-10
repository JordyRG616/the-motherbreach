using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitMovement : FormationMovement
{
    public override EnemyMovementType Type { get; protected set; } = EnemyMovementType.Orbit;

    [SerializeField] private float speed;
    [SerializeField] private float eccentricity;
    public static int sign = -1;
    private int _sign;

    protected override void Start()
    {
        sign *= -1;
        _sign = sign;
        base.Start();
    }

    public override void DoMovement(Rigidbody2D body, Vector3 target)
    {

        body.velocity = Vector2.zero;
        var direction = target - transform.position;
        direction = direction.normalized;
        var perpendicular = Vector2.Perpendicular(direction) * eccentricity;

        var finalDirection = ((Vector2)direction + (perpendicular * _sign));

        RotateChildren(finalDirection);
        body.AddForce(finalDirection * speed * 100);
    }
}
