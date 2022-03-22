using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class ArtilleryEffect : ActionEffect
{
    [SerializeField] private float initialBulletSpeed;
    [SerializeField] private float initialBurstSize;

    public override Stat specializedStat => Stat.BulletSpeed;

    public override Stat secondaryStat => Stat.Rate;

    public override void SetData()
    {
        StatSet.Add(Stat.BulletSpeed, initialBulletSpeed);
        SetBulletSpeed();
        StatSet.Add(Stat.Rate, initialBurstSize);
        SetRate();

        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetBulletSpeed();
        SetRate();
    }

    private void SetBulletSpeed()
    {
        var main = shooterParticle.main;
        main.startSpeed = StatSet[Stat.BulletSpeed];
    }

    private void SetRate()
    {
        var emission = shooterParticle.emission;
        var burst = emission.GetBurst(0);
        burst.cycleCount = (int)StatSet[Stat.Rate];
        emission.SetBurst(0, burst);
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    public override string DescriptionText()
    {
        string description = "shoots " + StatColorHandler.StatPaint(StatSet[Stat.Rate].ToString()) + " bullets per second. Each bullet deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage on hit";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        return StatColorHandler.StatPaint("next level:") + " damage +" + (nextLevel * 5) + "%";
    }

    public override void LevelUp(int toLevel)
    {
        var damage = StatSet[Stat.Damage];
        damage *= 1f + (((float)toLevel * 5) / 100);
        SetStat(Stat.Damage, damage);
    }
}
