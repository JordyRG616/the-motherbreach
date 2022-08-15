using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeMovement : FormationMovement
{
    [SerializeField] private float eccentricity;
    [SerializeField] private float speed;

    public override EnemyMovementType Type { get; protected set;} = EnemyMovementType.Flee;

    public override void DoMovement(Rigidbody2D body, Vector3 target)
    {
        body.velocity = Vector2.zero;
        var direction = target - transform.position;
        direction = -direction.normalized;
        var perpendicular = -Vector2.Perpendicular(direction) * eccentricity;

        var finalDirection = ((Vector2)direction + perpendicular);

        RotateChildren(finalDirection);
        body.AddForce(finalDirection * speed * 100);
    }
}
