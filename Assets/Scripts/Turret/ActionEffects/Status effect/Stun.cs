using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : StatusEffect
{
    public override Keyword Status => Keyword.Stun;

    protected override void ApplyEffect()
    {
        target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        target.GetComponent<EnemyAttackController>().Sleeping = true;
    }

    protected override void ExtraInitialize(params float[] parameters)
    {
        
    }

    protected override void RemoveEffect()
    {
        target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        target.GetComponent<EnemyAttackController>().Sleeping = false;
    }
}
