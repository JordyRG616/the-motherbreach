using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoicEffect : BaseEffectTemplate
{
    [SerializeField] [Range(0, 1)] private float percentage;
    

    public override void ApplyEffect()
    {
        associatedController.RaiseHealthByPercentage(percentage);
    }


    public override string DescriptionText()
    {
        string description = "Increases the health of this turret by " + percentage * 100 + "%";
        return description;
    }
}
