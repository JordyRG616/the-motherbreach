using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeController : BossController
{
    [SerializeField] private float distanceToChase;
    [SerializeField] private float distanceToFlee;


    protected override void ManageStates(int phaseOrder)
    {
        var distance = ship.transform.position - transform.position;
        if(distance.magnitude < distanceToFlee)
        {
            var fleeState = GetComponent<Interceptor_Disengage>();
            ChangeStates(fleeState);
            return;
        }
        if (distance.magnitude > distanceToChase)
        {
            var chaseState = GetComponent<Interceptor_MoveState>();
            ChangeStates(chaseState);
            return;
        }

        var moveState = GetComponent<Forge_OrbitState>();
        ChangeStates(moveState);
    }

    protected override void PhaseUpgrade(int phaseOrder)
    {

    }
}
