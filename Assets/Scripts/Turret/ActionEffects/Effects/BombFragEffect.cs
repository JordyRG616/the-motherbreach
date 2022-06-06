
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

    public override void RaiseInitialSpecializedStat(float percentage)
    {
        
    }

    public override void RaiseInitialSecondaryStat(float percentage)
    {
        
    }
}
