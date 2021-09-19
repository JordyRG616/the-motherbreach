using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackPatternTemplate : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    public abstract IEnumerator Sequence(List<AttackController> attackers);

    public abstract event EventHandler OnSequenceEnd;

    public float GetCooldown()
    {
        return cooldown;
    }
}
