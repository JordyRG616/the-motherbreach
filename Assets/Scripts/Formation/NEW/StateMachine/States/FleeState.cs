using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : FormationState
{
    public enum ChildrenBehaviour {None, Attack, RandomAttack}

    private Vector3 ogPosition;
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    [SerializeField] private ChildrenBehaviour childrenBehaviour;
    private Vector3 fleePosition;
    private Dictionary<EnemyManager, Vector3> childrenPositions = new Dictionary<EnemyManager, Vector3>();
    [SerializeField] [Range(0, 3f)] private float threshhold;

    protected override void HandleChildren(EnemyManager[] children, float step)
    {

        for(int i = 0; i < children.Length; i++)
        {
            var vector = fleePosition - transform.position;   

            var direction = Vector2.Perpendicular(vector) * Mathf.Pow(-1, i);

            children[i].transform.localPosition -= (Vector3)direction.normalized * (threshhold - (step / 10)) * speed;
            children[i].GetComponent<EnemyRotationController>().LookAtShip();
            if(step <= threshhold * 0.7) children[i].transform.localPosition -= (Vector3)vector.normalized;

        }

    }

    protected override void OnStateEnter()
    {
        ogPosition = transform.position;
        float angle = GetAngle(target.position, ogPosition);// + (180 * Mathf.Deg2Rad);
        fleePosition = (target.position + new Vector3(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle)));

        var enemies = GetComponentsInChildren<EnemyManager>();
        childrenPositions.Clear();


        foreach(EnemyManager enemy in enemies)
        {
            childrenPositions.Add(enemy, enemy.transform.position);
            enemy.GetComponent<MovableEntity>().Anchor();

            if(childrenBehaviour == ChildrenBehaviour.Attack) 
            {
                enemy.GetComponent<EnemyAttackController>().SetTarget(target.gameObject);
                enemy.GetComponent<EnemyAttackController>().Attack();
            }

            if(childrenBehaviour == ChildrenBehaviour.RandomAttack)
            {
                enemy.GetComponent<EnemyAttackController>().SetTarget(target.gameObject);

                var rdm = Random.Range(0, 1f);
                if(rdm >= 0.5f) enemy.GetComponent<EnemyAttackController>().Attack();
            }
        }

        transform.position = fleePosition;

        // foreach(EnemyManager _enemy in enemies)
        // {
        //     if(childrenPositions.ContainsKey(_enemy)) transform.position = childrenPositions[_enemy];
        // }

        base.OnStateEnter();
    }

    private float GetAngle(Vector3 a, Vector3 b)
    {
        var sub = a - b;
        var angle = Mathf.Atan2(sub.y, sub.x);
        return angle;
    }

    protected override void OnStateTick(float step)
    {
        transform.position = Vector2.LerpUnclamped(ogPosition, fleePosition, step/10 * speed);
    }


    protected override void OnStateExit()
    {
        foreach(EnemyManager enemy in GetComponentsInChildren<EnemyManager>())
        {
            enemy.GetComponent<MovableEntity>().LiftAnchor();
            enemy.GetComponent<EnemyAttackController>().Stop();
        }

        base.OnStateExit();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if(target != null) Gizmos.DrawLine(target.position, fleePosition);
    }

}
