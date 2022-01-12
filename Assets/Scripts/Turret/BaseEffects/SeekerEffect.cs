using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerEffect : BaseEffectTemplate
{

    public override void HandleLevelEffect(object sender, LevelUpArgs e)
    {
        if(e.toLevel == 3)
        {
            foreach(ActionEffect shooter in associatedController.GetShooters())
            {
                shooter.GetShooterSystem().gameObject.AddComponent<SeekerEffect>();
            }
        }
    }

    public override void ApplyEffect()
    {
        // foreach(ActionEffect shooter in associatedController.GetShooters())
        // {
        //     shooter.GetShooterSystem().gameObject.AddComponent<SeekerEffect>();
        // }
    }

    public override string DescriptionText()
    {
        string description = "At level 3, add SEEKING effect to this turret.";
        return description;
    }
}
