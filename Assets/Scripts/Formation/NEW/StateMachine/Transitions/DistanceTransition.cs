using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTransition : Transition
{

    public enum DistanceTransitionMode {GreaterThan, LesserThan}
    private DistanceTransitionMode mode;
    private float distance;

    public DistanceTransition(FormationState from, FormationState to, DistanceTransitionMode mode, float distance)
    {
        fromState = from;
        toState = to;
        this.mode = mode;
        this.distance = distance;
    }

    public override bool Trigger()
    {
        float _distance = Vector2.Distance(fromState.transform.position, fromState.GetTarget().position);
        if(mode == DistanceTransitionMode.GreaterThan && _distance > distance) return true;
        if(mode == DistanceTransitionMode.LesserThan && _distance < distance) return true;
        return false;
    }

    
}
