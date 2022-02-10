using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntMovement : FormationMovement
{
    [SerializeField] private float speed;

    public override EnemyMovementType Type { get; protected set; } = EnemyMovementType.Hunt;

    public override void DoMovement(Rigidbody2D body, Vector3 target)
    {
        body.velocity = Vector2.zero;
        var direction = target - transform.position;
        direction = direction.normalized;

        RotateChildren(direction);
        body.AddForce(direction * speed * 100);
    }
}
