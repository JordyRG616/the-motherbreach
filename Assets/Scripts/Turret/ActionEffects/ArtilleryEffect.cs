using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryEffect : ActionEffect
{
    [SerializeField] private float initialBulletSpeed;
    [SerializeField] private float initialBurstSize;

    protected override void SetData()
    {
        StatSet.Add(Stat.BulletSpeed, initialBulletSpeed);
        SetBulletSpeed();
        StatSet.Add(Stat.BurstSize, initialBurstSize);
        SetBurstSize();

        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetBulletSpeed();
        SetBurstSize();
    }

    private void SetBulletSpeed()
    {
        var main = shooterParticle.main;
        main.startSpeed = StatSet[Stat.BulletSpeed];
    }

    private void SetBurstSize()
    {
        var main = shooterParticle.main;
        main.duration = StatSet[Stat.BurstSize];
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    public override string DescriptionText()
    {
        string description = "Shoots " + StatSet[Stat.BurstSize] + " bullets. Each bullet deals a damage of " + StatSet[Stat.Damage] + " on hit.";
        return description;
    }

    public override void LevelUp(int toLevel)
    {
        var damage = StatSet[Stat.Damage];
        damage *= 1f + (((float)toLevel * 5) / 100);
        SetStat(Stat.Damage, damage);
    }
}
