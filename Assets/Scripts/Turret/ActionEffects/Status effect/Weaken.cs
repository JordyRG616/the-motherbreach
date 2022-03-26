using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weaken : StatusEffect
{
    private float percentage;
    private List<ActionEffect> shooters = new List<ActionEffect>();
    private Dictionary<ActionEffect, float> ogDamages = new Dictionary<ActionEffect, float>();


    public override Keyword Status => Keyword.Weaken;

    protected override void ApplyEffect()
    {
        shooters = target.GetComponents<ActionEffect>().ToList();

        foreach(ActionEffect shooter in shooters)
        {
            ogDamages.Add(shooter, shooter.StatSet[Stat.Damage]);
            shooter.SetStat(Stat.Damage, ogDamages[shooter] * (1 - percentage));
        }
    }

    protected override void ExtraInitialize(params float[] parameters)
    {

        percentage = parameters[0];
    }

    protected override void RemoveEffect()
    {
        foreach(ActionEffect shooter in shooters)
        {
            shooter.SetStat(Stat.Damage, ogDamages[shooter]);
        }
    }
}
