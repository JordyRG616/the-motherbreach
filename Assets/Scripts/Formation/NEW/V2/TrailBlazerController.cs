using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBlazerController : FormationMovementController
{
    [SerializeField] private float splitTime = .4f;
    [SerializeField] private float fleeTime;
    [SerializeField] private float distanceToSplit;
    private float timer;
    private bool countTime;
    private bool fleeing;

    protected override void ManageStates()
    {
        if(fleeing && timer >= fleeTime)
        {
            countTime = false;
            fleeing = false;
            timer = 0;
            RegisterMovement(EnemyMovementType.Orbit);
        }
        var distance = Vector2.Distance(transform.position, ship.transform.position);
        if(distance <= distanceToSplit)
        {
            RegisterMovement(EnemyMovementType.TrueOrbit);
            countTime = true;
            // var splitTimer = splitCooldown + splitTime;
        }
        if(timer >= splitTime)
        {
            countTime = true;
            timer = 0;
            fleeing = true;
            RegisterMovement(EnemyMovementType.Flee);
        }
    }

    void Update()
    {
        if(countTime) timer += Time.deltaTime;
        Debug.Log(timer);
    }
}
