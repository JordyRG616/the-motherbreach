using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredderEffect : BaseEffectTemplate
{
    private ShipManager ship;

    void Awake()
    {
        ship = FindObjectOfType<ShipManager>();
    }

    public override void ApplyEffect()
    {
        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            if(shooter.GetClass() == WeaponClass.Spawner)
            {
                var ogStat = shooter.StatSet[Stat.Capacity];
                shooter.SetStat(Stat.Capacity, ogStat + ShotgunCount());
            } 
            else if(shooter.GetClass() == WeaponClass.Bomber)
            {
                var ogStat = shooter.StatSet[Stat.Projectiles];
                shooter.SetStat(Stat.Projectiles, ogStat + ShotgunCount());
            }
            else
            {
                var ogStat = shooter.StatSet[Stat.BurstSize];
                shooter.SetStat(Stat.BurstSize, ogStat + ShotgunCount());
            }
        }
    }

    private int ShotgunCount()
    {
        var weapons = ship.GetWeapons();
        int count = 0;

        foreach(ActionController weapon in weapons)
        {
            if(weapon.GetClasses().Contains(WeaponClass.Shotgun))
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
