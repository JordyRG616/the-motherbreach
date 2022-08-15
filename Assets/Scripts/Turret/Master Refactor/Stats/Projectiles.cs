using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : TurretStat
{
    public override float Value { get => _value; protected set => _value = value; }

    protected override void SetValue(float value)
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var emission = shooter.emission;
            var burst = emission.GetBurst(0);
            burst.count = value;
            emission.SetBurst(0, burst);
        }
    }
}
