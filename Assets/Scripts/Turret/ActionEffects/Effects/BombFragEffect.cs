using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFragEffect : ActionEffect
{
    public override Stat specializedStat => Stat.Damage;

    public override Stat secondaryStat => Stat.Damage;

    public override void Initiate()
    {
        base.Initiate();
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        var damage = GetComponentInParent<ActionEffect>().StatSet[Stat.Damage];
        SetStat(Stat.Damage, damage);
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    public override string DescriptionText()
    {
        return "";
    }

    public override void LevelUp(int toLevel)
    {
        
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public override string DescriptionText(out Keyword keyword)
    {
        return base.DescriptionText(out keyword);
    }

    public override string upgradeText(int nextLevel)
    {
        return base.upgradeText(nextLevel);
    }

    protected override void ClearShots(object sender, GameStateEventArgs e)
    {
        base.ClearShots(sender, e);
    }

    public override void SetData()
    {
        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
    }

    public override void ReceiveTarget(GameObject parentTarget)
    {
        base.ReceiveTarget(parentTarget);
    }

    public override void Shoot()
    {
        base.Shoot();
    }

    protected override void ManageSFX()
    {
        base.ManageSFX();
    }

    public override void StopShooting()
    {
        base.StopShooting();
    }

    public override void RotateShoots()
    {
        base.RotateShoots();
    }

    public override void Update()
    {
        base.Update();
    }

    protected override void ApplyStatusEffect<T>(HitManager target, float duration, params float[] parameters)
    {
        base.ApplyStatusEffect<T>(target, duration, parameters);
    }
}
