using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoicEffect : BaseEffectTemplate
{
    [SerializeField] private float percentage;
    

    public override void ApplyEffect()
    {
        //var integrityManager = GetComponentInParent<IntegrityManager>();
        float totalPercentage = turretManager.Level * percentage;
        associatedController.RaiseHealthByPercentage(totalPercentage);
    }


    public override string DescriptionText()
    {
        string description = "Increases the health of this turret by 2% per level at the end of each wave.";
        return description;
    }
}
