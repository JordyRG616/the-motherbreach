using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoActionEffect : ActionEffect
{
    [SerializeField] private ParticleSystem death;
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
        if(toLevel == 3 || toLevel == 5) RaiseBurst();
        else RaiseDamage();
    }

    private void RaiseBurst()
    {
        var module = death.emission;
        var burst = module.GetBurst(0);
        burst.count = new ParticleSystem.MinMaxCurve(burst.count.constant + 10);
        module.SetBurst(0, burst);
    }

    private void RaiseDamage()
    {
        StatSet[Stat.Damage] *= 1.25f;
    }
}
