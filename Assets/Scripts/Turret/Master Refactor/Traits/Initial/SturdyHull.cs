using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Traits/Initial/Sturdy Hull", fileName = "Sturdy Hull")]
public class SturdyHull : Trait
{
    [Range(0, 1)] public float percentage;

    public override void ApplyEffect(Weapon weapon)
    {
        if (weapon.HasStat<Health>(out var health))
        {
            Debug.Log(percentage);
            health.ApplyPercentage(percentage);
        }
    }

    public override string Description()
    {
        return "Raise the integrity of this turret by " + percentage * 100 + "%";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (SturdyHull)ScriptableObject.CreateInstance(type);

        instance.percentage = percentage;

        return instance;
    }
}
