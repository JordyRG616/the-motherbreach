using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterState : FormationState
{
    [SerializeField] private Vector2[] positions;
    [SerializeField] private Transform[] enemies;
    private WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(0.01f);

    protected override void OnStateEnter()
    {

        for(int i = 0; i < enemies.Length; i++)
        {
           StartCoroutine(ResetPosition(enemies[i], positions[i]));
        }

        base.OnStateEnter();
    }

    private IEnumerator ResetPosition(Transform enemy, Vector2 position)
    {
        float step = 0;
        Vector2 origin = enemy.localPosition;

        while(step <= 1)
        {
            Vector2 newPos = Vector2.Lerp(origin, position, step);
            enemy.localPosition = newPos;

            step += 0.05f;
            
            yield return waitTime;
        }

        enemy.GetComponent<EnemyRotationController>().LookAtShip();

        enemy.GetComponent<EnemyAttackController>().Attack();
    }

    protected override void OnStateTick(float step)
    {
        return;
    }

    protected override void OnStateExit()
    {
        foreach(Transform enemy in enemies)
        {
            enemy.GetComponent<EnemyAttackController>().Stop();
        }

        base.OnStateExit();
    }
}
