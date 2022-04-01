using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedMovement : FormationMovement
{
    [SerializeField] private bool allowInertia;
    [SerializeField] private bool fixChildren;
    public override EnemyMovementType Type { get; protected set; } = EnemyMovementType.Fixed;

    public override void DoMovement(Rigidbody2D body, Vector3 target)
    {
        if(!allowInertia) body.velocity = Vector2.zero;
        var direction = target - transform.position;
        RotateChildren(direction);
        if(fixChildren) FixChildren();
    }
}
