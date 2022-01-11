using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FormationState : MonoBehaviour
{
    protected Transform target;
    protected List<Transition> transitions = new List<Transition>();
    private WaitForSeconds tickTime = new WaitForSeconds(0.01f);
    protected float step;
    protected FSM_Controller controller;

    protected virtual void Awake()
    {
        target = FindObjectOfType<ShipManager>().transform;
        controller = GetComponent<FSM_Controller>();
    }

    public virtual void Enter()
    {
        OnStateEnter();
    }

    public virtual void Exit()
    {
        OnStateExit();
    }

    protected virtual void OnStateEnter()
    {
        controller.RegisterState(this);
        step = 0;
        StartCoroutine(Tick());
    }

    protected abstract void OnStateTick(float step);

    protected virtual void OnStateExit()
    {
        StopAllCoroutines();
    }

    protected virtual IEnumerator Tick()
    {
        while(true)
        {
            OnStateTick(step);

            CheckTransitions();

            var children = GetComponent<FormationManager>().children;
            HandleChildren(children.ToArray(), step);

            step += 0.1f;

            yield return tickTime;
        }

    }

    protected void CheckTransitions()
    {
        foreach(Transition transition in transitions)
        {
            if(transition.Trigger())
            {
                transition.Forward();
            }
        }
    }

    protected abstract void HandleChildren(EnemyManager[] children, float step);


    public Transform GetTarget()
    {
        return target;
    }

    public void AddTransition(Transition transition)
    {
        transitions.Add(transition);
    }

    public void ClearTransitions()
    {
        transitions.Clear();
    }

    public float StepValue()
    {
        return step;
    }
}
