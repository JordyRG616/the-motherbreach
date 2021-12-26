using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RogueController : MonoBehaviour
{
    private Transform target;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    private float speed = 1;
    private EnemyAttackController attackController;
    private float distance = 5;

    public void Initialize(float speed, float distance)
    {
        attackController = GetComponent<EnemyAttackController>();
        GetTarget();
        attackController.SetTarget(target.gameObject);

        StartCoroutine(Move());

        this.speed = speed;
        this.distance  = distance;
    }

    private void GetTarget()
    {
        var target = FindObjectOfType<ShipManager>();
        this.target = target.transform;
    }

    private IEnumerator Move()
    {
        float step = 0;

        while(true)
        {
            if(Vector2.Distance(target.transform.position, transform.position) > distance)
            {
                transform.position += (Vector3)ReturnFollowPosition() * speed / 100;
            }

            if(Vector2.Distance(target.transform.position, transform.position) <= distance)
            {
                attackController.Attack();
                transform.position += (Vector3)ReturnOrbitPosition() * speed / 100;
            }


            Rotate();

            step += 1f;

            yield return waitTime;
        }
    }

    private Vector2 ReturnFollowPosition()
    {
        Vector2 direction = target.transform.position - transform.position;

        return direction.normalized;
    }

    private Vector2 ReturnOrbitPosition()
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        direction = Vector2.Perpendicular(direction);

        return direction;

    }

    private void Rotate()
    {
        GetComponent<EnemyRotationController>().LookAtShip();
    }
}
