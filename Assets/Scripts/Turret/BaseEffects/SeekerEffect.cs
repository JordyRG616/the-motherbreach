using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerEffect : BaseEffectTemplate
{
    public override void ApplyEffect()
    {
        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            shooter.GetShooterSystem().gameObject.AddComponent<SeekerEffect>();
        }
    }

    public override string DescriptionText()
    {
        string description = "";
        return description;
    }
}
