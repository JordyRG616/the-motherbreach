using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBlazerController : FormationMovementController
{
    [SerializeField] private float fleeDuration = .4f;
    [SerializeField] private float fleeDistance;
    [SerializeField] private float distanceToShoot;
    [SerializeField] private float shootDuration;
    private float timer;
    private bool countTime;
    private bool fleeing;
    private bool shooting;

    protected override void ManageStates()
    {
        if(fleeing && timer >= fleeDuration)
        {
            fleeing = false;
            countTime = false;
            timer = 0;
        }
        if(shooting && timer >= shootDuration)
        {
            shooting = false;
            timer = 0;
            fleeing = true;
            RegisterMovement(EnemyMovementType.Flee);
        }
        var distance = Vector2.Distance(transform.position, ship.transform.position);
        
        if(distance > distanceToShoot && !shooting && !fleeing)
        {
            RegisterMovement(EnemyMovementType.Orbit);
            // var splitTimer = splitCooldown + splitTime;
        }
        if(distance <= distanceToShoot && !shooting && !fleeing)
        {
            RegisterMovement(EnemyMovementType.Cone);
            countTime = true;
            shooting = true;
        }
        
    }

    void Update()
    {
        if(countTime) timer += Time.deltaTime;
    }
}
