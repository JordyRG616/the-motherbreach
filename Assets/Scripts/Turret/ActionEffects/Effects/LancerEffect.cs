using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;
using System;

public class LancerEffect : ActionEffect
{
    [SerializeField] private float initialBulletSpeed;
    [SerializeField] private float initialBulletSize;

    public override void SetData()
    {
        StatSet.Add(Stat.BulletSpeed, initialBulletSpeed);
        SetBulletSpeed();
        StatSet.Add(Stat.BulletSize, initialBulletSize);
        SetBulletSize();

        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetBulletSpeed();
        SetBulletSize();
    }

    private void SetBulletSpeed()
    {
        var main = shooterParticle.main;
        main.startSpeed = StatSet[Stat.BulletSpeed];
    }

    private void SetBulletSize()
    {
        var main = shooterParticle.main;
        main.startSize = StatSet[Stat.BulletSize];
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    public override string DescriptionText()
    {
        string description = "shoots a piercing arrow that deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage on hit";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " arrow size +" + (nextLevel * 10) + "%";
        else return StatColorHandler.StatPaint("next level:") + " damage +10%";
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 3 || toLevel == 5)
        {
            GainSize(toLevel * 10);
        }
        else
        {
            GainDamage();
        }
    }

    private void GainDamage()
    {
        var damage = StatSet[Stat.Damage];
        damage *= 1.1f;
        SetStat(Stat.Damage, damage);
    }

    private void GainSize(float percentage)
    {
        var size = StatSet[Stat.BulletSize];
        size *= 1 + (percentage / 100);
        SetStat(Stat.BulletSize, size);
    }
}
