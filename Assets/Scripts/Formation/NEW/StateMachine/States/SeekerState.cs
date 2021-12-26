using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerState : FormationState
{
    private Vector3 ogPosition;
    private Vector3 middlePoint;
    [SerializeField] private float speed;

    protected override void HandleChildren(EnemyManager[] children, float step)
    {
        foreach(EnemyManager child in children)
        {
            
            var distance = child.transform.position - transform.position;
            var perpendicular = Vector2.Perpendicular(distance);

            child.GetComponent<MovableEntity>().AccelerationOverride(perpendicular - (Vector2)distance / 10);

            child.GetComponent<EnemyRotationController>().LookAtShip();
        }

    }

    protected override void OnStateEnter()
    {
        ogPosition = transform.position;

        base.OnStateEnter();
    }

    protected override void OnStateTick(float step)
    {
        middlePoint = target.position - transform.position;

        // var ab = Vector2.Lerp(transform.position, middlePoint, step * speed / 100);
        // var bc = Vector2.Lerp(middlePoint, target.position, step * speed / 100);

        transform.position = Vector2.Lerp(ogPosition, target.position, step * speed / 100);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(middlePoint, .6f);
    }
}
