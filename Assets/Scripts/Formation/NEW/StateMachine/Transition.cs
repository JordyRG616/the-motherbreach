using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Transition 
{
    protected FormationState fromState;
    protected FormationState toState;

    public abstract bool Trigger();

    public void Forward()
    {
        fromState.Exit();
        toState.Enter();
    }

    public void Reverse()
    {
        toState.Exit();
        fromState.Enter();
    }
}
