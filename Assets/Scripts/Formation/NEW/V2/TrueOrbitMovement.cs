using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueOrbitMovement : FormationMovement
{
    [SerializeField] private float speed;

    public override EnemyMovementType Type { get; protected set; } = EnemyMovementType.TrueOrbit;

    public override void DoMovement(Rigidbody2D body, Vector3 target)
    {
        body.velocity = Vector2.zero;
        var direction = target - transform.position;
        direction = direction.normalized;
        var perpendicular = Vector2.Perpendicular(direction);

        var finalDirection = (perpendicular);

        RotateChildren(finalDirection);
        body.AddForce(finalDirection * speed * 100);
    }

}
