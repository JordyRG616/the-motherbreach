using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMediator : MonoBehaviour
{
    [SerializeField] private ActionEffect associatedEffect;

    public void PassTarget(HitManager target)
    {   
        associatedEffect.totalEffect(target);
    }
}
