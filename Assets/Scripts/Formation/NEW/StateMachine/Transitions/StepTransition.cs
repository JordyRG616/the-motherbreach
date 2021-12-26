using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTransition : Transition
{
    private float duration;

    public StepTransition(FormationState from, FormationState to, float duration)
    {
        fromState = from;
        toState = to;
        this.duration = duration;
    }

    public override bool Trigger()
    {
        // Debug.Log(fromState.StepValue());
        if(fromState.StepValue() >= duration) return true;
        return false;
    }
}
