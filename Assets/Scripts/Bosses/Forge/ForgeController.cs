using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeController : BossController
{
    [SerializeField] private float distanceToChase;
    [SerializeField] private float distanceToFlee;
    [Header("Second phase upgrade")]
    [SerializeField] private List<GameObject> phaseTwoFormations;
    [Header("Third phase upgrade")]
    [SerializeField] [Range(0, 1)] private float cooldownReduction;
    private bool secondPhaseOn, thirdPhaseOn;


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
        switch(phaseOrder)
        {
            case 0:
            break;
            case 1:
                SecondPhaseUpgrade();
            break;
            case 2:
                ThirdPhaseUpgrade();
            break;
        }
    }

    private void ThirdPhaseUpgrade()
    {
        if(thirdPhaseOn) return;
        GetComponent<ForgeEnemyManager>().ReduceCooldown(cooldownReduction);
        thirdPhaseOn = true;
    }

    private void SecondPhaseUpgrade()
    {
        if(secondPhaseOn) return;
        GetComponent<ForgeEnemyManager>().phaseTwoOn = true;
        GetComponent<ForgeEnemyManager>().ReceiveNewFormations(phaseTwoFormations);
        secondPhaseOn = true;
    }
}
