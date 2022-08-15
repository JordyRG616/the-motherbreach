using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Traits/Initial/Extra Override", fileName = "Extra Override")]
public class ExtraOverride : Trait
{
    public override void ApplyEffect(Weapon weapon)
    {
        var dormentStats = weapon.GetDormentStats();
        var rdm = Random.Range(0, dormentStats.Count);
        weapon.ExposeDormentStat(dormentStats[rdm]);
    }

    public override string Description()
    {
        return " Unlocks a random stat of the weapon";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (ExtraOverride)ScriptableObject.CreateInstance(type);

        return instance;
    }
}
