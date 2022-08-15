using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Traits/Initial/Fast Reload Tool", fileName = "Fast Reload Tool")]
public class FastReload : Trait
{
    [Range(0, 1)] public float percentage;

    public override void ApplyEffect(Weapon weapon)
    {
        if(weapon.HasStat<Cooldown>(out var cooldown))
        {
            cooldown.ApplyPercentage(-percentage);
        }
    }

    public override string Description()
    {
        return "Reduce the cooldown of this turret by " + percentage * 100 + "%";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (FastReload)ScriptableObject.CreateInstance(type);

        instance.percentage = percentage;

        return instance;
    }
}
