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
                float ogLevel = shooter.StatSet[ActionStat.DroneLevel];
                shooter.SetStat(ActionStat.DroneLevel, ogLevel + 1); 
            } 
            else
            {
                float ogDamage = shooter.StatSet[ActionStat.Damage];
                shooter.SetStat(ActionStat.Damage, ogDamage * (1 + percentage));
            }
        }
    }

}
