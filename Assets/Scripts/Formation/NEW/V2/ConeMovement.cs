using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeMovement : FormationMovement
{
    [SerializeField] private bool allowInertia;
    [SerializeField] private float angleVariation;
    [SerializeField] private float rotationSpeed;
    private float timer;

    public override EnemyMovementType Type { get; protected set; } = EnemyMovementType.Cone;

    public override void DoMovement(Rigidbody2D body, Vector3 target)
    {
        if(!allowInertia) body.velocity = Vector2.zero;
        var direction = target - transform.position;
        Debug.Log(VariableAngle(angleVariation));
        RotateChildren(direction, VariableAngle(transform.rotation.z));
    }

    private float VariableAngle(float angle)
    {
        return angle + (Mathf.Cos(timer * rotationSpeed) * angleVariation);
    }

    protected override void FixedUpdate()
    {
        if(rotationSpeed > 0) timer += Time.fixedDeltaTime;
        base.FixedUpdate();
    }
}
