using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterceptorArtillery : ActionEffect
{
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
