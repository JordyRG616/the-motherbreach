using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SluggingEffect : BaseEffectTemplate
{

    public override void HandleLevelEffect(object sender, LevelUpArgs e)
    {
        if(e.toLevel == 3)
        {
            foreach(ActionEffect shooter in associatedController.GetShooters())
            {
                shooter.totalEffect += AddSlug;
            }
        }
    }

    public override void ApplyEffect()
    {
        // foreach(ActionEffect shooter in associatedController.GetShooters())
        // {
        //     shooter.totalEffect += AddSlug;
        // }
    }

    public void AddSlug(HitManager hitManager)
    {
        if(hitManager.TryGetComponent<Slug>(out Slug slug))
        {
            return;
        } else
        {
            hitManager.gameObject.AddComponent<Slug>();
        }
    }
    
    public override string DescriptionText()
    {
        string description = "At level 3, add SLUG effect to this turret.";
        return description;
    }
}
