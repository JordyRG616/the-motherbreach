using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBlazerController : FormationMovementController
{
    [SerializeField] private float fleeDuration = .4f;
    [SerializeField] private float fleeDistance;
    private float timer;
    private bool fleeing;

    protected override void ManageStates()
    {
        if (fleeing)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= fleeDuration) fleeing = false;
            return;
        }

        var distance = Vector2.Distance(transform.position, ship.transform.position);

        if(distance <= fleeDistance)
        {
            RegisterMovement(EnemyMovementType.Flee);
            fleeing = true;
            timer = 0;
        }
        else
        {
            RegisterMovement(EnemyMovementType.Orbit);
        }
        

        //if(fleeing && timer >= fleeDuration)
        //{
        //    fleeing = false;
        //    countTime = false;
        //    timer = 0;
        //}
        //if(shooting && timer >= shootDuration)
        //{
        //    shooting = false;
        //    timer = 0;
        //    fleeing = true;
        //    RegisterMovement(EnemyMovementType.Flee);
        //}
        
        //if(distance > distanceToShoot && !shooting && !fleeing)
        //{
        //    RegisterMovement(EnemyMovementType.Orbit);
        //    // var splitTimer = splitCooldown + splitTime;
        //}
        //if(distance <= distanceToShoot && !shooting && !fleeing)
        //{
        //    RegisterMovement(EnemyMovementType.Cone);
        //    countTime = true;
        //    shooting = true;
        //}
        
    }
}
