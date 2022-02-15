using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weaken : StatusEffect
{
    private float percentage;
    private float[] ogDamage;
    private List<ActionEffect> shooters = new List<ActionEffect>();


    public override Keyword Status => Keyword.Weaken;

    protected override void ApplyEffect()
    {
        int i = 0;

        foreach(ActionEffect shooter in shooters)
        {
            ogDamage[i] = shooter.StatSet[Stat.Damage];
            shooter.SetStat(Stat.Damage, ogDamage[i] * percentage);
            i++;
        }
    }

    protected override void ExtraInitialize(params float[] parameters)
    {
        shooters = target.GetComponent<ActionController>().GetShooters();
        ogDamage = new float[shooters.Count];

        percentage = parameters[0];
    }

    protected override void RemoveEffect()
    {
        int i = 0;

        foreach(ActionEffect shooter in shooters)
        {
            shooter.SetStat(Stat.Damage, ogDamage[i]);
            i++;
        }
    }
}
