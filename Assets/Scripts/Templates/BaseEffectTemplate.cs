using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffectTemplate : MonoBehaviour
{
    [SerializeField] protected BaseEffectTrigger trigger;
    protected ActionController associatedController;

    public abstract void ApplyEffect(List<ActionEffect> shooters);

}
