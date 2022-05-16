using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelUpData 
{
    public enum UpgradeType{Flat, Percentage}

    public int level;
    public Stat stat;
    public UpgradeType type;
    public float amount;

    public void ApplyLevelUp(ActionEffect shooter)
    {
        var value = shooter.StatSet[stat];
        if(type == UpgradeType.Flat) value += amount;
        else value *= (1 + amount);
        
        shooter.SetStat(stat, value);

    }
}
