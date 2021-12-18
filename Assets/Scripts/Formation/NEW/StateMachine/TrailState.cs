using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailState : FormationState
{
    [SerializeField] private List<Transform> Enemies;
    [SerializeField] private float range;
    private Vector3 targetPosition, ogPosition;

    [SerializeField] private float speed;

    protected override void OnStateEnter()
    {

        ogPosition = transform.position;

        targetPosition = -transform.position;

        step = 0;

        for(int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].localPosition = new Vector3(-1 + i, (Mathf.Pow(-1, i) * 0.5f) - 0.5f);
            Enemies[i].GetComponent<EnemyRotationController>().StartRotation(Mathf.Sign(GetComponent<IdleState>().GetSpeed()));
            Enemies[i].GetComponent<EnemyAttackController>().Attack();
        }

        base.OnStateEnter();
    }

    protected override void OnStateTick(float step)
    {
        foreach(Transform enemy in Enemies)
        {
            float sin = Mathf.Sin(step) * range;
            enemy.transform.localPosition += new Vector3(sin, 0);
        }

        float sign = Mathf.Sign(GetComponent<IdleState>().GetSpeed());

        Vector3 newPos;

        if(sign <= 0 )  newPos = Vector3.SlerpUnclamped(ogPosition, Vector2.Perpendicular(targetPosition), step * speed);
        else    newPos = Vector3.SlerpUnclamped(ogPosition, -Vector2.Perpendicular(targetPosition), step * speed);

        transform.position = newPos;

    }

    protected override void OnStateExit()
    {
        foreach(Transform enemy in Enemies)
        {
            enemy.GetComponent<EnemyAttackController>().Stop();
        }

        GetComponent<IdleState>().ResetRadiuses(transform.position, step);

        base.OnStateExit();
    }

   
}
