using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weaken : StatusEffect
{
    private float percentage;
    private float ogDamage;
    private EnemyActionEffect shooter;


    public override Keyword Status => Keyword.Weaken;

    protected override void ApplyEffect()
    {
        shooter = target.GetComponent<EnemyActionEffect>();
        ogDamage = shooter.StatSet[Stat.Damage];
        Debug.Log("working");
        shooter.SetStat(Stat.Damage, ogDamage * (1 - percentage));
    }

    protected override void ExtraInitialize(params float[] parameters)
    {

        percentage = parameters[0];
    }

    protected override void RemoveEffect()
    {
        shooter.SetStat(Stat.Damage, ogDamage);
    }
}
