using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;
using System;

public class SentryGunEffect : ActionEffect
{
    [SerializeField] private float rateMultiplier;
    [SerializeField] private float initialBulletSpeed;

    public override Stat specializedStat => Stat.BulletSpeed;

    public override Stat secondaryStat => Stat.Rate;

    private string extraInfo = "for 3 seconds";

    public override void SetData()
    {
        StatSet.Add(Stat.Rate, rateMultiplier);
        SetRateMultiplier();
        StatSet.Add(Stat.BulletSpeed, initialBulletSpeed);
        SetBulletSpeed();

        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetRateMultiplier();
        SetBulletSpeed();
    }

    private void SetRateMultiplier()
    {
        var emission = shooterParticle.emission;
        emission.rateOverTimeMultiplier = StatSet[Stat.Rate];
    }

    private void SetBulletSpeed()
    {
        var main = shooterParticle.main;
        main.startSpeed = StatSet[Stat.BulletSpeed];
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    public override string DescriptionText()
    {
        string description = "shoots " + StatColorHandler.StatPaint(StatSet[Stat.Rate].ToString()) + " bullets per second " + extraInfo + ". Each bullet deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage on hit";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 2 || nextLevel == 4) return StatColorHandler.StatPaint("next level:") + " rest -10%";
        else return StatColorHandler.StatPaint("next level:") + " rate of fire +15%";
    }

    public override void LevelUp(int toLevel)
    {
        var main = shooterParticle.main;
        main.loop = true;

        extraInfo = "continuously";
    }

    public override void RemoveLevelUp()
    {
        var main = shooterParticle.main;
        main.loop = false;

        extraInfo = "for 3 seconds";
    }

    public override void RaiseInitialSpecializedStat(float percentage)
    {
        initialBulletSpeed *= 1 + percentage;
    }

    public override void RaiseInitialSecondaryStat(float percentage)
    {
        rateMultiplier *= 1 + percentage;
    }
}
