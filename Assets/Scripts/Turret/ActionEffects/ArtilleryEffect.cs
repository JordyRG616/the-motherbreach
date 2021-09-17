using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryEffect : ActionEffect
{

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-data.bulletDamage);
    }
}
