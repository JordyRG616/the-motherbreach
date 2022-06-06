using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

[CreateAssetMenu(menuName = "Data/Reward/Subroutines/Attribute by class", fileName = "New Subroutine")]
public class BasicAttributeByClass : ShipSubroutine
{
    [SerializeField] private WeaponClass weaponClass;
    private int count = 0;
    private float ogValue;

    public override void Initiate()
    {
        base.Initiate();

        ogValue = value;
    }

    public override string RequirementText()
    {
        return "-";
    }

    protected override bool SubroutineCondition()
    {
        var turrets = ship.turrets;
        count = 0;

        foreach (TurretManager turret in turrets)
        {
            if (turret.actionController.GetWeaponClass() == weaponClass) count++;
        }

        value = ogValue * count;

        if (count == 0) return false;
        else return true;
    }
}
