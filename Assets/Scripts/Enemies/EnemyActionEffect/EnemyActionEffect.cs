using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionEffect : ActionEffect
{
    public override Stat specializedStat { get;}
    public override Stat secondaryStat { get;}

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    public override string DescriptionText()
    {
        string description = "";
        return description;
    }

    public override void LevelUp(int toLevel)
    {
        
    }
}
