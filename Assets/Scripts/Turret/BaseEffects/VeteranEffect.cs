using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeteranEffect : BaseEffectTemplate
{
 
    public override void ApplyEffect()
    {
        for (int i = 0; i < 3; i++)
        {
            Invoke("LevelUp", i/10f);
        }
    }

    private void LevelUp()
    {
        turretManager.LevelUp();
    }

    public override string DescriptionText()
    {
        string description = "Immediately level up this turret three times";
        return description;
    }
}
