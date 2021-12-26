using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamerState : FormationState
{
    [SerializeField] private float strayDistance;
    [SerializeField] private float roamDuration;
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] private float distance;
    private Vector2 prePosition;
    private Vector3 targetPrePosition;
    private Vector2 direction;
    private Vector2 childrenMovement;

    protected override void OnStateEnter()
    {
        foreach(EnemyRotationController enemy in GetComponentsInChildren<EnemyRotationController>())
        {
            enemy.StartRotation(1);
        }

        CalculateDirection();

        base.OnStateEnter();
    }


    protected override void HandleChildren(EnemyManager[] children, float step)
    {
        foreach(EnemyManager child in children)
        {
            var distance = transform.position - child.transform.position;
            if(distance.magnitude >= strayDistance)
            {
                child.GetComponent<MovableEntity>().ClearForces();
                child.GetComponent<MovableEntity>().AccelerationOverride(distance * 10);
            }
            else
            {
                distance = child.transform.position - transform.position;
                var perpendicular = Vector2.Perpendicular(distance);

                childrenMovement = perpendicular - (Vector2)distance / 10;

                child.GetComponent<MovableEntity>().AccelerationOverride(childrenMovement);
            }
        }
    }

    protected override void OnStateTick(float step)
    {
        step = Mathf.Clamp(step/10, 0, roamDuration - 0.1f);
        Debug.Log(step);

        var _dir = ((Vector3)direction * distance) + targetPrePosition;

        transform.position = Vector3.Lerp(prePosition, _dir, speedCurve.Evaluate((step) / roamDuration));

        if(Mathf.Approximately(step, roamDuration - 0.1f))
        {
            CalculateDirection();
        }
    }

    private void CalculateDirection()
    {
        prePosition = transform.position;
        targetPrePosition = target.position;
        direction = Vector2.Perpendicular(transform.position - target.transform.position).normalized;
        step = 0;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, direction);
    }
}
