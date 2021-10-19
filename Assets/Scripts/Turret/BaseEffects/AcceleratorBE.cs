using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorBE : BaseEffectTemplate
{


    public override void ApplyEffect()
    {
        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            float value = shooter.GetData().Cooldown / 2;
            shooter.GetData().SetStat(ActionStat.Cooldown, value);
        }

        UpdateControllerStats();
    }
}
