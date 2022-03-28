using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    protected HitManager target;
    protected float duration;
    public abstract Keyword Status {get;}
    private float countdown;


    public void Initialize(HitManager target, float duration, params float[] parameters)
    {
        BasicInitialize(target, duration);
        ExtraInitialize(parameters);

        InitiateEffect();

    }

    protected virtual void BasicInitialize(HitManager target, float duration)
    {
        this.target = target;
        this.duration = duration;
    }

    protected abstract void ExtraInitialize(params float[] parameters);

    protected abstract void ApplyEffect();

    protected abstract void RemoveEffect();

    protected virtual void InitiateEffect()
    {
        ApplyEffect();
        target.ReceiveEffect(this);        
    }

    protected virtual void FixedUpdate()
    {
        countdown += Time.fixedDeltaTime;
        if(countdown >= duration)
        {
            RemoveEffect();
            target.RemoveEffect(this);
            Destroy(this);
        }
    }
}
