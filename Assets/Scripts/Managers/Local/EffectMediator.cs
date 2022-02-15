using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMediator : MonoBehaviour
{
    public ActionEffect associatedEffect;

    public void PassTarget(HitManager target, out float damage)
    {   
        associatedEffect.totalEffect(target);
        damage = associatedEffect.StatSet[Stat.Damage];
    }
}
