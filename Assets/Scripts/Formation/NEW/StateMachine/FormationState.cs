using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FormationState : MonoBehaviour
{
    [SerializeField] protected float stateDuration;
    private WaitForSecondsRealtime tickTime = new WaitForSecondsRealtime(0.01f);
    private IEnumerator _tick;
    protected float step;


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
        _tick = Tick();
        StartCoroutine(_tick);
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

            step += 0.1f;

            yield return tickTime;
        }

    }

    public float GetDuration()
    {
        return stateDuration;
    }
}
