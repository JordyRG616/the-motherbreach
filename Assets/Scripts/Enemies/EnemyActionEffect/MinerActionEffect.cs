using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerActionEffect : ActionEffect
{
    public override Stat specializedStat { get;}
    public override Stat secondaryStat { get;}

    public override void Initiate()
    {
        base.Initiate();

    }

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
        RaiseEmission();
        // else RaiseLifetime();
    }

    private void RaiseEmission()
    {
        var emission = shooterParticle.emission;
        emission.rateOverDistance = new ParticleSystem.MinMaxCurve(emission.rateOverDistance.constant + .1f);
    }

    private void RaiseLifetime()
    {
        var main = shooterParticle.main;
        main.startLifetime = new ParticleSystem.MinMaxCurve(main.startLifetime.constant + 1);
    }
}
