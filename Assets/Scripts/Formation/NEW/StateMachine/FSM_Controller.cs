using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSM_Controller : MonoBehaviour
{
    [SerializeField] protected FormationState entryState;
    protected Stack<FormationState> stateHistory = new Stack<FormationState>();
    [SerializeField] private bool generateDebugMessage;

    [ContextMenu("Generate Debug History")]
    public void DebugHistory()
    {
        string debug = string.Empty;
        foreach(FormationState state in stateHistory)
        {
            debug += state.GetType().ToString() + "; ";
        }
        Debug.Log(debug);
    }

    protected virtual void Start()
    {
        SetTransitions();

        entryState.Enter();
    }

    public void RegisterState(FormationState state)
    {
        stateHistory.Push(state);
        if(generateDebugMessage) Debug.Log(state.GetType().ToString());
    }

    protected abstract void SetTransitions();
}
