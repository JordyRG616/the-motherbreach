using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IdleState : FormationState
{
    [SerializeField] private float speed;
    [SerializeField] private float strayDistance;
    Vector3 movement = Vector3.zero;

    private Vector3 CalculateVector()
    {
        Vector3 distance = target.position - transform.position;
        Vector3 perpendicular = Vector2.Perpendicular(distance) * speed / 100;

        return perpendicular;
    }

    protected override void OnStateTick(float step)
    {
        transform.position += CalculateVector();
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

                movement = perpendicular - (Vector2)distance / 10;

                child.GetComponent<MovableEntity>().AccelerationOverride(movement);
            }
        }
    }

    protected override void OnStateEnter()
    {
        foreach(EnemyRotationController enemy in GetComponentsInChildren<EnemyRotationController>())
        {
            enemy.StartRotation(-1);
        }

        base.OnStateEnter();
    }

    protected override void OnStateExit()
    {
        foreach(EnemyRotationController enemy in GetComponentsInChildren<EnemyRotationController>())
        {
            enemy.StopRotation();
        }

        base.OnStateExit();
    }
}
