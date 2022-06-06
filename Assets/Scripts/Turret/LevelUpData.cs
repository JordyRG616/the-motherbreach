using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelUpData 
{
    public enum UpgradeType{Flat, Percentage, Special}

    public int level;
    public Stat stat;
    public UpgradeType type;
    public float amount;

    public void ApplyLevelUp(ActionEffect shooter)
    {
        if(type == UpgradeType.Special)
        {
            SpecialLevelUp(shooter);
            return;
        }

        var value = shooter.StatSet[stat];
        if(type == UpgradeType.Flat) value += amount;
        else value *= (1 + amount);
        
        shooter.SetStat(stat, value);
    }

    private void SpecialLevelUp(ActionEffect shooter)
    {
        shooter.LevelUp(level);
    }
}
