using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Traits/Initial/General Enhancement", fileName = "General Enhancement")]
public class GeneralEnhancement : Trait
{
    public int upgradePoints;

    public override void ApplyEffect(Weapon weapon)
    {

        var manager = weapon.GetComponentInParent<TurretManager>();
        manager.upgradePoints += upgradePoints;
    }

    public override string Description()
    {
        return "This turret receives " + upgradePoints + " upgrade points";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (GeneralEnhancement)ScriptableObject.CreateInstance(type);

        instance.upgradePoints = upgradePoints;

        return instance;
    }
}
