using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEffect : ActionEffect
{
    [SerializeField] private float initialProjectiles;
    [SerializeField] private float initalBulletSize;
    [SerializeField] private ParticleSystem subShooter;

    protected override void SetData()
    {
        StatSet.Add(Stat.Projectiles, initialProjectiles);
        SetProjectiles();
        StatSet.Add(Stat.BulletSize, initalBulletSize);
        SetBulletSize();
        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetProjectiles();
        SetBulletSize();
    }

    private void SetProjectiles()
    {
        var emission = subShooter.emission;
        ParticleSystem.Burst burst = new ParticleSystem.Burst(0.0001f, StatSet[Stat.Projectiles]);
        emission.SetBurst(0, burst);
    }

    private void SetBulletSize()
    {
        var main = shooter.main;
        main.startSize = StatSet[Stat.BulletSize];
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }
}
