using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChargeMovement : FormationMovement
{
    [SerializeField] private float speed;
    private Vector2 direction;

    public override EnemyMovementType Type { get; protected set; } = EnemyMovementType.Charge;

    public override void Initiate()
    {
        direction = GetComponent<Rigidbody2D>().velocity.normalized;
    }

    public override void DoMovement(Rigidbody2D body, Vector3 target)
    {
        body.velocity = Vector2.zero;

        RotateChildren(direction);
        body.AddForce(direction * speed, ForceMode2D.Impulse);
    }


}
