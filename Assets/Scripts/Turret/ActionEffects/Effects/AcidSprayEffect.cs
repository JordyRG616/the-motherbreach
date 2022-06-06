using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class AcidSprayEffect : ActionEffect
{
    [SerializeField] private float acidDuration;
    [SerializeField] private float tickInterval;
    private FMOD.Studio.EventInstance instance;

    public override Stat specializedStat => Stat.Duration;
    public override Stat secondaryStat => Stat.Efficiency;

    private int directDamageModifier = 0;

    public override void SetData()
    {
        StatSet.Add(Stat.Duration, acidDuration);
        SetDuration();
        StatSet.Add(Stat.Efficiency, tickInterval);
        SetEfficiency();
        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetDuration();
        SetEfficiency();
    }

    private void SetDuration()
    {
        acidDuration = StatSet[Stat.Duration];
    }

    private void SetEfficiency()
    {
        tickInterval = 1 - StatSet[Stat.Efficiency];
        if(tickInterval < .1f)
        {
            tickInterval = .1f;
        }
    }

    public override void Shoot()
    {
        StartCoroutine(PlaySFX(shooterParticle.main.duration));
        base.Shoot();
    }

    private IEnumerator PlaySFX(float duration)
    {
        yield return new WaitForSeconds(duration + 1);

        sfxInstances.ForEach(x => StopSFX(x));
        sfxInstances.Clear();
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-(StatSet[Stat.Damage] * directDamageModifier) / 2);
        ApplyStatusEffect<Acid>(hitManager, acidDuration, new float[] {StatSet[Stat.Damage], tickInterval});
    }

    public override string DescriptionText()
    {
        string description = "releases a conic spray that applies " + KeywordHandler.KeywordPaint(keyword) + " (deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage over time for " + StatColorHandler.DamagePaint(StatSet[Stat.Duration].ToString()) + " seconds)";
        if(directDamageModifier == 1)
        {
            description += ". also deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage] / 2) + " damage on contact";
        }
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " acid duration + 20%";
        else return StatColorHandler.StatPaint("next level:") + " damage + 10%";
        
    }

    public override void LevelUp(int toLevel)
    {
        directDamageModifier = 1;
    }

    public override void RemoveLevelUp()
    {
        directDamageModifier = 0;
    }

    public override void RaiseInitialSpecializedStat(float percentage)
    {
        acidDuration *= 1 + percentage;
    }

    public override void RaiseInitialSecondaryStat(float percentage)
    {
        tickInterval *= 1 - percentage;
    }
}
