using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorBE : BaseEffectTemplate
{


    public override void ApplyEffect()
    {
        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            // float value = shooter.GetStat().Cooldown / 2;
            // shooter.GetStat().SetStat(ActionStat.Cooldown, value);
        }

        UpdateControllerStats();
    }
}
