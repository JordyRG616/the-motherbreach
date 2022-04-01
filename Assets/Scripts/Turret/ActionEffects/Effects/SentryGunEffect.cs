using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;
using System;

public class SentryGunEffect : ActionEffect
{
    [SerializeField] private float rateMultiplier;
    [SerializeField] private float duration;

    public override Stat specializedStat => Stat.Rate;

    public override Stat secondaryStat => Stat.Duration;

    public override void SetData()
    {
        StatSet.Add(Stat.Rate, rateMultiplier);
        SetRateMultiplier();
        StatSet.Add(Stat.Duration, duration);
        SetDuration();

        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetRateMultiplier();
        SetDuration();
    }

    private void SetRateMultiplier()
    {
        var emission = shooterParticle.emission;
        emission.rateOverTimeMultiplier = StatSet[Stat.Rate];
    }

    private void SetDuration()
    {
        var main = shooterParticle.main;
        main.duration = StatSet[Stat.Duration];
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    public override string DescriptionText()
    {
        string description = "shoots " + StatColorHandler.StatPaint(StatSet[Stat.Rate].ToString()) + " bullets per second for " + StatColorHandler.StatPaint(StatSet[Stat.Duration].ToString()) + ". Each bullet deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage on hit";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 2 || nextLevel == 4) return StatColorHandler.StatPaint("next level:") + " rest -10%";
        else return StatColorHandler.StatPaint("next level:") + " rate of fire +15%";
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 2 || toLevel == 4) ReduceRest();
        else GainRate();
    }

    private void ReduceRest()
    {
        var rest = StatSet[Stat.Rest];
        rest *= .9f;
        SetStat(Stat.Rest, rest);
    }

    private void GainRate()
    {
        var rate = StatSet[Stat.Rate];
        rate *= 1.15f;
        SetStat(Stat.Rate, rate);
    }
}
