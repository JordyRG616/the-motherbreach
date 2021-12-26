using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBlazer : FSM_Controller
{
    [SerializeField] private float distanceToFlee, durationOfFlee;

    protected override void SetTransitions()
    {
        var roam = GetComponent<RoamerState>();
        var flee = GetComponent<FleeState>();

        var idleToFlee = new DistanceTransition(roam, flee, DistanceTransition.DistanceTransitionMode.LesserThan, distanceToFlee);
        roam.AddTransition(idleToFlee);

        var fleeToIdle = new StepTransition(flee, roam, durationOfFlee);
        flee.AddTransition(fleeToIdle);
    }

    
}
