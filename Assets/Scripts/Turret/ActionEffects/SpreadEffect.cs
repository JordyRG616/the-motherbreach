using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadEffect : ActionEffect
{

    protected override void Awake()
    {
        shooter = GetComponent<ParticleSystem>();
        
        shooter.Stop();
    }

    public override void Shoot()
    {
        shooter.Play();
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        throw new System.NotImplementedException();
    }
}
