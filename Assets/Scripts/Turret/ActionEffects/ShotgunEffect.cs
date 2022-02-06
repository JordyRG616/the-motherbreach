using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class ShotgunEffect : ActionEffect
{
    [SerializeField] private float initialbulletSpeed;
    [SerializeField] private float initialProjectiles;

    public override void SetData()
    {
        StatSet.Add(Stat.BulletSpeed ,initialbulletSpeed);
        SetBulletSpeed();
        StatSet.Add(Stat.Projectiles, initialProjectiles);
        SetProjectileCount();
    
        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetBulletSpeed();
        SetProjectileCount();
        
    }

    private void SetProjectileCount()
    {
        var newBurst = new ParticleSystem.Burst(0.0001f, StatSet[Stat.Projectiles]);
        shooterParticle.emission.SetBurst(0, newBurst);

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
        string description = "shoots " + StatColorHandler.StatPaint(StatSet[Stat.Projectiles].ToString()) + " bullets that deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage each";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " projectiles + 1";
        else return StatColorHandler.StatPaint("next level:") + " damage + 20%";
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 3 || toLevel == 5) GainProjectile();
        else GainDamage();
    }

    private void GainDamage()
    {
        var damage = StatSet[Stat.Damage];
        damage *= 1.2f;
        SetStat(Stat.Damage, damage);
    }

    private void GainProjectile()
    {
        var projectiles = StatSet[Stat.Projectiles];
        projectiles += 1;
        SetStat(Stat.Projectiles, projectiles);
    }
}
