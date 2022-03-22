using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class BlunderbussEffect : ActionEffect
{
    [SerializeField] private float initialFuseTime;
    [SerializeField] private float initalBulletSize;
    [SerializeField] private ParticleSystem subShooter;
    [SerializeField] private ActionEffect fragEffect;
    [SerializeField] [FMODUnity.EventRef] private string explosionSFX;

    public override Stat specializedStat => Stat.FuseTime;

    public override Stat secondaryStat => Stat.Size;

    public override void Initiate()
    {
        base.Initiate();

        fragEffect.Initiate();
    }

    public override void SetData()
    {
        StatSet.Add(Stat.FuseTime, initialFuseTime);
        SetFuseTime();
        StatSet.Add(Stat.Size, initalBulletSize);
        SetBulletSize();
        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetFuseTime();
        SetBulletSize();
    }

    private void SetFuseTime()
    {
        var main = shooterParticle.main;
        main.startLifetime = StatSet[Stat.FuseTime];
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
        string description = "shoots a bomb that explodes after " + StatColorHandler.StatPaint(StatSet[Stat.FuseTime].ToString()) + " and that deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage to every enemy in the blast";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " fuse time + 15%";
        else return StatColorHandler.StatPaint("next level:") + " blast size + 10%";
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 3 || toLevel == 5) ReduceFuseTime();
        else GainSize();
    }

    private void ReduceFuseTime()
    {
        var fuseTime = StatSet[Stat.FuseTime];
        fuseTime *= 1.15f;
        SetStat(Stat.FuseTime, fuseTime);
    }

    public void GainSize()
    {
        var size = StatSet[Stat.Size];
        size *= 1.1f;
        SetStat(Stat.Size, size);
    }
}
