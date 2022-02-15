using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProdigyEffect : BaseEffectTemplate
{
    [SerializeField] private int expRequirement;
    private int currentExp;

    public override void ApplyEffect()
    {
        if(turretManager.Level == turretManager.maxLevel) return;
        currentExp ++;
        if(currentExp == expRequirement)
        {
            turretManager.LevelUp();
            currentExp = 0;
        }
    }

    public override string DescriptionText()
    {
        return "gain 1 experience. if this turret has " + expRequirement + " experience, it gains a level. (current exp: " + currentExp + ")";
    }
}
