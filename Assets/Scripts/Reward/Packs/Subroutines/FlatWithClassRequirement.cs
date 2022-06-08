using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

[CreateAssetMenu(menuName = "Data/Reward/Subroutines/Flat with requirement", fileName = "New Subroutine")]
public class FlatWithClassRequirement : ShipSubroutine
{
    [SerializeField] private WeaponClass weaponClass;
    [SerializeField] private int requirement;

    public override string RequirementText()
    {
        return "at least " + requirement + " " + weaponClass.ToSplittedString();
    }

    protected override bool SubroutineCondition()
    {
        var turrets = ship.turrets;
        int count = 0;

        foreach (TurretManager turret in turrets)
        {
            //if (turret.actionController.GetWeaponClass() == weaponClass) count++;
        }

        if (count >= requirement) return true;
        return false;
    }
}
