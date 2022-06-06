using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFieldEffect : ActionEffect
{
    public override Stat specializedStat => Stat.Damage;

    public override Stat secondaryStat => Stat.Rest;

    public override void ApplyEffect(HitManager hitManager)
    {
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
