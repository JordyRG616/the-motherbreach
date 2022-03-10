using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class AcidSprayEffect : ActionEffect
{
    [SerializeField] private float acidDuration;
    [SerializeField] private float tickInterval = .25f;
    private FMOD.Studio.EventInstance instance;

    public override Stat specializedStat => Stat.Duration;
    public override Stat secondaryStat => Stat.Efficiency;

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
        tickInterval = StatSet[Stat.Efficiency];
    }

    public override void Shoot()
    {
        // StartCoroutine(PlaySFX(StatSet[Stat.Duration]));
        AudioManager.Main.RequestSFX(onShootSFX, out sfxInstance);
        shooterParticle.Play();
    }

    private IEnumerator PlaySFX(float duration)
    {
        AudioManager.Main.RequestSFX(onShootSFX, out var instance);

        yield return new WaitForSeconds(duration + 1);

        AudioManager.Main.StopSFX(instance);
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        // hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
        ApplyStatusEffect<Acid>(hitManager, acidDuration, new float[] {StatSet[Stat.Damage], tickInterval});
    }

    public override string DescriptionText()
    {
        string description = "releases a conic spray of " + KeywordHandler.KeywordPaint(keyword) + " that deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage overtime for " + StatColorHandler.DamagePaint(StatSet[Stat.Duration].ToString()) + " seconds";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " acid duration + 20%";
        else return StatColorHandler.StatPaint("next level:") + " damage + 10%";
        
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 3 || toLevel == 5)
        {
            GainDuration();
        }
        else
        {
            GainDamage();
        }
    }

    private void GainDuration()
    {
        var _duration = StatSet[Stat.Duration];
        _duration *= 1.2f;
        SetStat(Stat.Duration, _duration);
    }

    private void GainDamage()
    {
        var damage = StatSet[Stat.Damage];
        damage *= 1.1f;
        SetStat(Stat.Damage, damage);
    }
}
