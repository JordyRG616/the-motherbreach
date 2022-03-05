using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class SharkEffect : ActionEffect
{
    [SerializeField] private float stunDuration;
    [SerializeField] private float initalBulletSize;
    [SerializeField] private float initialBulletSpeed;

    public override void SetData()
    {
        StatSet.Add(Stat.BulletSpeed, initialBulletSpeed);
        SetBulletSpeed();
        StatSet.Add(Stat.BulletSize, initalBulletSize);
        SetBulletSize();
        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetBulletSpeed();
        SetBulletSize();
    }

    private void SetBulletSize()
    {
        var main = shooterParticle.main;
        main.startSizeMultiplier = StatSet[Stat.BulletSize];
    }

    private void SetBulletSpeed()
    {
        var velocity = shooterParticle.velocityOverLifetime;
        velocity.yMultiplier = StatSet[Stat.BulletSpeed];
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
        ApplyStatusEffect<Stun>(hitManager, stunDuration);
    }

    
    public override string DescriptionText()
    {
        string description = "shoots a burst of shots that deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage on hit and applies " + KeywordHandler.KeywordPaint(keyword);
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        return StatColorHandler.StatPaint("next level:") + " damage + 10%\nbullet size + 5%";
    }

    public override void LevelUp(int toLevel)
    {
        GainDamage();
        GainSize();
    }

    private void GainSize()
    {
        var size = StatSet[Stat.BulletSize];
        size *= 1.05f;
        SetStat(Stat.BulletSize, size);
    }

    private void GainDamage()
    {
        var damage = StatSet[Stat.Damage];
        damage *= 1.1f;
        SetStat(Stat.Damage, damage);
    }
}
