using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheapEffect : BaseEffectTemplate
{
    [SerializeField] [Range(-1, 0)] private float percentage;

    public override void ApplyEffect()
    {
        associatedController.RaiseHealthByPercentage(percentage);
    }

    public override string DescriptionText()
    {
        return "reduce the cost of this turret by " + cost + " and the health by " + percentage * 100 + "%";
    }
}
