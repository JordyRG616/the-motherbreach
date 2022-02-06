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
            if(shooter.GetClass() == WeaponClass.Spawner)
            {
                float ogLevel = shooter.StatSet[Stat.DroneLevel];
                shooter.SetStat(Stat.DroneLevel, ogLevel + 1); 
            }
            else
            {
                float ogDamage = shooter.StatSet[Stat.Damage];
                shooter.SetStat(Stat.Damage, ogDamage * (1 + percentage));
            }
        }
    }

    public override string DescriptionText()
    {
        string description = "raises the " + StatColorHandler.DamagePaint("damage") + " of this turret in " + StatColorHandler.StatPaint((percentage * 100).ToString()) + "%";
        return description;
    }

    public override string DescriptionTextByClass(WeaponClass weaponClass)
    {
        if(weaponClass == WeaponClass.Healer)
        {
            return "this turret heals " + StatColorHandler.StatPaint((percentage * 100).ToString()) + "% more points of damage";
        }
        return DescriptionText();
    }
}
