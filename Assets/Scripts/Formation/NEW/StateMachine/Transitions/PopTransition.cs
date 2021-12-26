using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopTransition : Transition
{
    public PopTransition(FormationState from, FormationState to)
    {
        fromState = from;
        toState = to;
    }

    public override bool Trigger()
    {
        return true;
    }
}
