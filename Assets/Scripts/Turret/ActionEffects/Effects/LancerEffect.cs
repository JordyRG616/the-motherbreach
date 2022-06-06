using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;
using System;

public class LancerEffect : ActionEffect
{
    [SerializeField] private float initialBulletSpeed;
    [SerializeField] private float initialBulletSize;
    private string extraInfo = "a piercing arrow";

    public override Stat specializedStat => Stat.BulletSpeed;

    public override Stat secondaryStat => Stat.Size;

    public override void SetData()
    {
        StatSet.Add(Stat.BulletSpeed, initialBulletSpeed);
        SetBulletSpeed();
        StatSet.Add(Stat.Size, initialBulletSize);
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
        main.startSize = StatSet[Stat.Size];
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    public override string DescriptionText()
    {
        string description = "shoots " + extraInfo + " that deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage on hit";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " arrow size +" + (nextLevel * 10) + "%";
        else return StatColorHandler.StatPaint("next level:") + " damage +10%";
    }

    public override void LevelUp(int toLevel)
    {
        shooterParticle.GetComponent<SeekerParticleComponent>().enabled = true;
        extraInfo = "a homing piercing arrow";
    }

    public override void RemoveLevelUp()
    {
        shooterParticle.GetComponent<SeekerParticleComponent>().enabled = false;        
        extraInfo = "a piercing arrow";
    }

    public override void RaiseInitialSpecializedStat(float percentage)
    {
        initialBulletSpeed *= 1 + percentage;
    }

    public override void RaiseInitialSecondaryStat(float percentage)
    {
        initialBulletSize *= 1 + percentage;
    }
}
