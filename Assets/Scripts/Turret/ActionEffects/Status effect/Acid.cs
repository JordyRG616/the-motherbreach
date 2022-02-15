using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : StatusEffect
{
    private float damage;
    private float frequency;
    private float timer;
    private bool active;

    public override Keyword Status => Keyword.Acid;

    protected override void ApplyEffect()
    {
        active = true;
    }

    protected override void ExtraInitialize(params float[] parameters)
    {
        damage = parameters[0];
        frequency = parameters[1];
    }

    protected override void RemoveEffect()
    {
        active = false;
        Destroy(this);
    }

    void FixedUpdate()
    {
        if(!active) return;
        timer += Time.fixedDeltaTime;
        if(timer >= frequency) 
        {
            target.HealthInterface.UpdateHealth(-damage);
            timer = 0;
        }
    }
}
