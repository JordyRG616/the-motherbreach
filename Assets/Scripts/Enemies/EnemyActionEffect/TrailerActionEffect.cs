using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerActionEffect : ActionEffect
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
        
    }

    private void RaiseSize()
    {
        var main = shooterParticle.main;
        main.startSize = new ParticleSystem.MinMaxCurve(main.startSize.constant + 1);
    }

    private void RaiseRate()
    {
        var emission = shooterParticle.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(emission.rateOverTime.constant + 5);
    }
}
