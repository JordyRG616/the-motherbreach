using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsPerSecond : TurretStat
{
    public override float Value { get => _value; protected set => _value = value; }

    protected override void SetValue(float value)
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var emission = shooter.emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(value);
        }
    }
}
