using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterceptorDeployer : DeployerActionEffect
{
    public override Stat specializedStat => Stat.Damage;

    public override Stat secondaryStat => Stat.Rest;


    public override void SetData()
    {
        StatSet.Add(Stat.Capacity, capacity);

        base.SetData();
    }

    public override void ApplyEffect(HitManager hitManager)
    {
    }

    public override string DescriptionText()
    {
        return "";
    }

    public override void LevelUp(int toLevel)
    {
    }
}
