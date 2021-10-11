using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorBE : BaseEffectTemplate
{


    public override void ApplyEffect(List<ActionEffect> shooters)
    {
        foreach(ActionEffect shooter in shooters)
        {
            float value = shooter.GetData().cooldown / 2;
            shooter.GetData().SetStat(ActionStat.Speed, value);
        }
    }
}
