using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class AgressiveEffect : BaseEffectTemplate
{
    [SerializeField] [Range(0, 1)] private float percentage;

    public override void ApplyEffect()
    {
        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            float ogDamage = shooter.StatSet[Stat.Damage];
            shooter.SetStat(Stat.Damage, ogDamage * (1 + percentage));
        }
    }

    public override string DescriptionText()
    {
        string description = "raises the " + StatColorHandler.DamagePaint("damage") + " of this turret in " + StatColorHandler.StatPaint((percentage * 100).ToString()) + "%";
        return description;
    }
}
