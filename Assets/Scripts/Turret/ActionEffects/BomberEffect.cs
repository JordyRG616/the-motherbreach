using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEffect : ActionEffect
{
    [SerializeField] private float initialProjectiles;
    [SerializeField] private float initalBulletSize;
    [SerializeField] private ParticleSystem subShooter;
    [SerializeField] private ActionEffect fragEffect;

    public override void Initiate()
    {
        base.Initiate();

        fragEffect.Initiate();
    }

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

    public override string DescriptionText()
    {
        string description = "Shoots two bombs that explodes in " + StatSet[Stat.Projectiles] + " that deals " + GetComponentInChildren<BombFragEffect>().StatSet[Stat.Damage] + " damage on hit, each.";
        return description;
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 5) GainDamage();
        else GainProjectile();
    }

    private void GainProjectile()
    {
        var projectiles = StatSet[Stat.Projectiles];
        projectiles += 1;
        SetStat(Stat.Projectiles, projectiles);
    }

    private void GainDamage()
    {
        var damage = GetComponentInChildren<BombFragEffect>().StatSet[Stat.Damage];
        damage *= 1.5f;
        GetComponentInChildren<BombFragEffect>().SetStat(Stat.Damage, damage);
    }
}
