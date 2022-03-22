using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class EnhancePerLevel : BaseEffectTemplate
{
    [SerializeField] [Range(0, 1)] private float percentage;

    public override void ApplyEffect()
    {
        var level = turretManager.Level;

        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            foreach(Stat stat in targetedStats)
            {
                if(shooter.StatSet.ContainsKey(stat))
                {
                    var value = shooter.StatSet[stat];
                    value *= 1 + (percentage * level);
                    shooter.SetStat(stat, value);
                }
            }
        }
    }

    public override string DescriptionText()
    {
        string container = "";
        foreach(Stat stat in targetedStats)
        {
            container += stat.ToString().ToLower() + "/";
        }
        container = container.Remove(container.Length -1);
        return "raises " + StatColorHandler.StatPaint(container) + " by " + StatColorHandler.StatPaint((percentage * 100).ToString()) + "% per level of this turret";
    }

    public override string DescriptionTextByStat(Stat stat)
    {
        var level = turretManager? turretManager.Level: 0;

        return "raises the " + StatColorHandler.StatPaint(stat.ToString().ToLower()) + " by " + StatColorHandler.StatPaint((percentage * 100).ToString()) + "% per level of this turret (level " + level + ": " +  StatColorHandler.StatPaint((percentage * 100 * level).ToString() + "%") + ")";
    }
}
