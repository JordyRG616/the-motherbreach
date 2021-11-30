using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryEffect : ActionEffect
{
    [SerializeField] private float initialBulletSpeed;
    [SerializeField] private float initialBurstSize;

    protected override void SetData()
    {
        StatSet.Add(ActionStat.BulletSpeed, initialBulletSpeed);
        SetBulletSpeed();
        StatSet.Add(ActionStat.BurstSize, initialBurstSize);
        SetBurstSize();

        base.SetData();
    }

    public override void SetStat(ActionStat statName, float value)
    {
        base.SetStat(statName, value);
        SetBulletSpeed();
        SetBurstSize();
    }

    private void SetBulletSpeed()
    {
        var main = shooter.main;
        main.startSpeed = StatSet[ActionStat.BulletSpeed];
    }

    private void SetBurstSize()
    {
        var main = shooter.main;
        main.duration = StatSet[ActionStat.BurstSize];
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[ActionStat.Damage]);
    }
}
