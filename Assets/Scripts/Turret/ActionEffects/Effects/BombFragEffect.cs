using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFragEffect : ActionEffect
{
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
}
