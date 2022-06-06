using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class BlunderbussEffect : ActionEffect
{
    [SerializeField] private float initialBlastDuration;
    [SerializeField] private float initalBulletSize;
    [SerializeField] private ParticleSystem subShooter;
    [SerializeField] private ActionEffect fragEffect;
    [SerializeField] [FMODUnity.EventRef] private string explosionSFX;
    private string extraInfo = "";

    public override Stat specializedStat => Stat.Size;

    public override Stat secondaryStat => Stat.Duration;

    public override void Initiate()
    {
        base.Initiate();

        fragEffect.Initiate();
    }

    public override void SetData()
    {
        StatSet.Add(Stat.Duration, initialBlastDuration);
        SetBlastDuration();
        StatSet.Add(Stat.Size, initalBulletSize);
        SetBulletSize();
        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetBlastDuration();
        SetBulletSize();
    }

    private void SetBlastDuration()
    {
        var main = subShooter.main;
        main.startLifetime = StatSet[Stat.Duration];
    }

    private void SetBulletSize()
    {
        var main = subShooter.main;
        main.startSizeMultiplier = StatSet[Stat.Size];
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        // hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    protected override void PlaySFX()
    {
        base.PlaySFX();
        // Invoke("PlayExplosion", shooterParticle.main.startLifetime.constant);
    }

    private void PlayExplosion()
    {
        AudioManager.Main.RequestSFX(explosionSFX);
    }

    public override string DescriptionText()
    {
        string description = "shoots a bomb that explodes after " + StatColorHandler.StatPaint(StatSet[Stat.Duration].ToString()) + " and deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage to every enemy in the blast";
        description += extraInfo;
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " fuse time + 15%";
        else return StatColorHandler.StatPaint("next level:") + " blast size + 10%";
    }

    public override void LevelUp(int toLevel)
    {
        subShooter.subEmitters.SetSubEmitterEmitProbability(0, .5f);
        extraInfo += ". Triggers an additional explosion after the first";
    }

    public override void RemoveLevelUp()
    {
        subShooter.subEmitters.SetSubEmitterEmitProbability(0, 0);
        extraInfo = "";
    }

    public override void RaiseInitialSpecializedStat(float percentage)
    {
        initalBulletSize *= 1 + percentage;
    }

    public override void RaiseInitialSecondaryStat(float percentage)
    {
        initialBlastDuration *= 1 + percentage;
    }
}
