using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SplitMovement : FormationMovement
{
    [SerializeField] private float force;
    [SerializeField] private float distance;
    private List<SpringJoint2D> joints;
    public override EnemyMovementType Type { get; protected set; } = EnemyMovementType.Split;
    private float step;
    private Vector2 direction = Vector2.zero;

    protected override void Start()
    {
        joints = GetComponents<SpringJoint2D>().ToList();
        base.Start(); 
    }

    public override void Initiate()
    {
        direction = Vector2.zero;
        step = 0;

        foreach(EnemyManager enemy in enemies)
        {
            var _body = enemy.GetComponent<Rigidbody2D>();
            _body.velocity = Vector2.zero;            
        }
    }

    public override void DoMovement(Rigidbody2D body, Vector3 target)
    {
        body.velocity = Vector2.zero;
        step += Time.fixedDeltaTime;
        // Debug.Log(step);
        foreach(SpringJoint2D joint in joints)
        {
            if(step <= 2 * Mathf.PI) joint.distance += Mathf.Sin(step) * distance;
        }

        if(direction == Vector2.zero) direction = (target - transform.position).normalized;

        body.AddForce(direction * force, ForceMode2D.Impulse);

        var sign = -1;

        foreach(EnemyManager enemy in enemies)
        {
            if(step <= Mathf.PI) 
            {
                var _body = enemy.GetComponent<Rigidbody2D>();
                // _body.velocity = Vector2.zero;
                _body.AddForce((direction + Vector2.Perpendicular(direction) * sign * 2) * 10);
                sign *= -1;
            }
        }

        RotateChildren(direction);
    }
}
