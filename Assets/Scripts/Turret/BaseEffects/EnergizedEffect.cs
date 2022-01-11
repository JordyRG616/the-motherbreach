using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergizedEffect : BaseEffectTemplate
{
    [SerializeField] [Range(0, 1)] private float percentage;
    private ShipManager ship;

    void Awake()
    {
        ship = FindObjectOfType<ShipManager>();
    }

    public override void ApplyEffect()
    {
        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            if(shooter.GetClass() == WeaponClass.Artillery || shooter.GetClass() == WeaponClass.Shotgun )
            {
                var ogStat = shooter.StatSet[Stat.BulletSpeed];
                ogStat *= 1 + (percentage * ArtilleryCount());
                shooter.SetStat(Stat.BulletSpeed, ogStat);
            } else
            {
                var ogStat = shooter.StatSet[Stat.Duration];
                ogStat *= 1 + (percentage * ArtilleryCount());
                shooter.SetStat(Stat.Duration, ogStat);
            }
        }
    }

    private int ArtilleryCount()
    {
        var weapons = ship.GetWeapons();
        int count = 0;

        foreach(ActionController weapon in weapons)
        {
            if(weapon.GetClasses().Contains(WeaponClass.Artillery))
            {
                count++;
            }
        }

        return count;
    }

    public override string DescriptionText()
    {
        string description = "";
        return description;
    }
}
