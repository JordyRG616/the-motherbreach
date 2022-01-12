using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeteranEffect : BaseEffectTemplate
{
 
    public override void ApplyEffect()
    {
        turretManager.LevelUp();
    }

    public override string DescriptionText()
    {
        string description = "Immediately level up this turret.";
        return description;
    }
}
