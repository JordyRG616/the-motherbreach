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
        var main = shooter.main;
        main.startSpeed = StatSet[Stat.BulletSpeed];
    }

    private void SetBurstSize()
    {
        var main = shooter.main;
        main.duration = StatSet[Stat.BurstSize];
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }
}
