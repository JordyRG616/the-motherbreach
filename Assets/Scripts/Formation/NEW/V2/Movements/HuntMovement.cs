using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntMovement : FormationMovement
{
    [SerializeField] private float speed;
    private float _speed;
    private float RisingSpeed
    {
        get
        {
            if(_speed < speed)
            {
                _speed += 0.05f;
            }
            return _speed;
        }
    }

    public override EnemyMovementType Type { get; protected set; } = EnemyMovementType.Hunt;

    public override void DoMovement(Rigidbody2D body, Vector3 target)
    {
        body.velocity = Vector2.zero;
        var direction = target - transform.position;
        direction = direction.normalized;

        RotateChildren(direction);
        body.AddForce(direction * RisingSpeed * 100);
    }
}
