using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;
using System;

public class CometEffect : ActionEffect
{
    [SerializeField] private List<ParticleSystem> subEmitters;
    [SerializeField] private float initialDuration;
    [SerializeField] private float initialBurstSize;

    public override Stat specializedStat => Stat.BurstCount;

    public override Stat secondaryStat => Stat.Duration;

    public override void SetData()
    {
        StatSet.Add(Stat.Duration, initialDuration);
        SetDuration();
        StatSet.Add(Stat.BurstCount, initialBurstSize);
        SetBurstSize();

        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetDuration();
        SetBurstSize();
    }

    private void SetDuration()
    {
        var main = shooterParticle.main;
        main.duration = StatSet[Stat.Duration];
    }

    private void SetBurstSize()
    {
        foreach(ParticleSystem emitter in subEmitters)
        {
            var main = emitter.emission;
            var burst = main.GetBurst(0);
            var minMax = new ParticleSystem.MinMaxCurve(1, StatSet[Stat.BurstCount]);
            burst.count = minMax;
        }
        
    }
    
    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    public override string DescriptionText()
    {
        return "shoots a volley of shots. each deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage and have a chance of shooting up to " + StatColorHandler.StatPaint(StatSet[Stat.BurstCount].ToString()) + " extra projectiles";
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 2 || nextLevel == 4) return StatColorHandler.StatPaint("next level:") + " 1 extra projectile\nduration + 10%";
        else return StatColorHandler.StatPaint("next level:") + " duration + 10%";
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 2 || toLevel == 4) GainBurstSize();
        GainDuration();
    }

    private void GainBurstSize()
    {
        var burst = StatSet[Stat.BurstCount];
        burst ++;
        SetStat(Stat.BurstCount, burst);
    }

    private void GainDuration()
    {
        var duration = StatSet[Stat.Duration];
        duration *= 1.1f;
        SetStat(Stat.Duration, duration);
    }

    public override void RaiseInitialSpecializedStat(float percentage)
    {
        
    }

    public override void RaiseInitialSecondaryStat(float percentage)
    {
        
    }
}
