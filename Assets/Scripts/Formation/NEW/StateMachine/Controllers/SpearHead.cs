using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearHead : FSM_Controller
{
    [SerializeField] private float distanceToSeek, distanceToIdle, distanceToFlee, durationOfFlee, deployInterval;
    private float deployStep = 0;

    protected override void Start()
    {
        base.Start();
        
        StartCoroutine(HandleDeploy());
    }
    
    protected override void SetTransitions()
    {
        var idle = GetComponent<IdleState>();
        var seeker = GetComponent<SeekerState>();
        var flee = GetComponent<FleeState>();

        var idleToSeeker = new DistanceTransition(idle, seeker, DistanceTransition.DistanceTransitionMode.GreaterThan, distanceToSeek);
        var idleToFlee = new DistanceTransition(idle, flee, DistanceTransition.DistanceTransitionMode.LesserThan, distanceToFlee);
        idle.AddTransition(idleToSeeker);
        idle.AddTransition(idleToFlee);

        var seekerToIdle = new DistanceTransition(seeker, idle, DistanceTransition.DistanceTransitionMode.LesserThan, distanceToIdle);
        seeker.AddTransition(seekerToIdle);

        var fleeToIdle = new StepTransition(flee, idle, durationOfFlee);
        flee.AddTransition(fleeToIdle);
    }

    private IEnumerator HandleDeploy()
    {
        while(gameObject.activeSelf)        
        {
            if(GetComponentsInChildren<EnemyManager>().Length > 0)
            {
                if(deployStep >= deployInterval)
                {
                    ActivateDeploy();
                    deployStep = 0;
                }

                deployStep += 0.01f;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    private void ActivateDeploy()
    {
        var poppedState = stateHistory.Peek();
        poppedState.Exit();

        var deploy = GetComponent<DeployRogueState>();
        var outOfDeploy = new PopTransition(deploy, poppedState);
        deploy.ClearTransitions();
        deploy.AddTransition(outOfDeploy);

        deploy.Enter();
    }
}
