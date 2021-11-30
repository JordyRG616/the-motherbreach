using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFragEffect : ActionEffect
{
    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[ActionStat.Damage]);
    }    
}
