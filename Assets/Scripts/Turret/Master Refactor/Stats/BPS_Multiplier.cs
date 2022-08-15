using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPS_Multiplier : BulletsPerSecond
{
    protected override void SetValue(float value)
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var emission = shooter.emission;
            emission.rateOverTimeMultiplier = value;
        }
    }
}
