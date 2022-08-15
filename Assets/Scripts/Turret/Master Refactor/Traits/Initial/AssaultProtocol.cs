using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Traits/Initial/Assault Protocol", fileName = "Assault Protocol")]
public class AssaultProtocol : Trait
{
    [Range(0, 1)] public float percentage;

    public override void ApplyEffect(Weapon weapon)
    {
        if (weapon.HasStat<Damage>(out var damage))
        {
            damage.ApplyPercentage(percentage);
        }
    }

    public override string Description()
    {
        return "Raise the damage of this turret by " + percentage * 100 + "%";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (AssaultProtocol)ScriptableObject.CreateInstance(type);

        instance.percentage = percentage;

        return instance;
    }
}
