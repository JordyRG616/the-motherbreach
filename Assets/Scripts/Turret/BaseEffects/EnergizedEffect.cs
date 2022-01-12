using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergizedEffect : BaseEffectTemplate
{
    [SerializeField] [Range(0, 1)] private float percentage;

    public override void ApplyEffect()
    {
        var level = turretManager.Level;

        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            if(shooter.GetClass() == WeaponClass.Artillery || shooter.GetClass() == WeaponClass.Shotgun )
            {
                var ogStat = shooter.StatSet[Stat.BulletSpeed];
                ogStat *= 1 + (percentage * level);
                shooter.SetStat(Stat.BulletSpeed, ogStat);
            } else
            {
                var ogStat = shooter.StatSet[Stat.Duration];
                ogStat *= 1 + (percentage * level);
                shooter.SetStat(Stat.Duration, ogStat);
            }
        }
    }

    public override string DescriptionText()
    {
        string description = "";
        return description;
    }
}
