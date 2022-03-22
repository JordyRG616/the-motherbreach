using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class StoicEffect : BaseEffectTemplate
{
    [SerializeField] [Range(0, 1)] private float percentage;
    

    public override void ApplyEffect()
    {
        associatedController.RaiseHealthByPercentage(percentage);
    }


    public override string DescriptionText()
    {
        string description = "Increases the " + StatColorHandler.HealthPaint("health") + " of this turret by " + StatColorHandler.StatPaint((percentage * 100).ToString()) + "%";
        return description;
    }
}
