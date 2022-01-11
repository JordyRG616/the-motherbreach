using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        string description = "Raises the damage of this turret in " + percentage * 100 + "%";
        return description;
    }
}
