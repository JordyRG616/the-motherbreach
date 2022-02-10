using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncendiaryEffect : BaseEffectTemplate
{
    public override void HandleLevelTrigger(object sender, LevelUpArgs e)
    {
        if(e.toLevel == 3)
        {
            foreach(ActionEffect shooter in associatedController.GetShooters())
            {
                shooter.totalEffect += AddBurn;
            }
        }
    }

    public override void ApplyEffect()
    {
        // foreach(ActionEffect shooter in associatedController.GetShooters())
        // {
        //     shooter.totalEffect += AddBurn;
        // }
    }

    public void AddBurn(HitManager hitManager)
    {
        if(hitManager.TryGetComponent<ChemicalBurn>(out ChemicalBurn burn))
        {
            return;
        } else
        {
            hitManager.gameObject.AddComponent<ChemicalBurn>();
        }
        
    }


    public override string DescriptionText()
    {
        string description = "At level 3, add CHEMICAL BURN effect to this turret.";
        return description;
    }
}
