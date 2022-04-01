using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneActionEffect : ActionEffect
{
    public override Stat specializedStat { get;}
    public override Stat secondaryStat { get;}

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    public override string DescriptionText()
    {
        string description = "";
        return description;
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 3 || toLevel == 5) RaiseSpeed();
        RaiseBurst();
    }

    private void RaiseBurst()
    {
        var module = shooterParticle.emission;
        var burst = module.GetBurst(0);
        burst.cycleCount += 1;
        module.SetBurst(0, burst);
    }

    private void RaiseSpeed()
    {
        var module = shooterParticle.velocityOverLifetime;
        module.radialMultiplier += 10;
    }
}
