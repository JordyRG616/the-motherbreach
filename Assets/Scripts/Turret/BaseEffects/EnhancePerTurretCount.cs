using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class EnhancePerTurretCount : BaseEffectTemplate
{

    [SerializeField] [Range(0, 1)] private float percentage;

    public override void ApplyEffect()
    {
        var count = ShipManager.Main.GetTurretCount();

        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            foreach(Stat stat in targetedStats)
            {
                if(shooter.StatSet.ContainsKey(stat))
                {
                    var value = shooter.StatSet[stat];
                    value *= 1 + (percentage * count);
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
            container += stat.ToString() + "/";
        }
        return "raises the " + container + " by " + StatColorHandler.StatPaint((percentage * 100).ToString()) + "% per turret on the ship";
    }

    public override string DescriptionTextByStat(Stat stat)
    {
        return "raises the " + StatColorHandler.StatPaint(stat.ToString().ToLower()) + " by " + StatColorHandler.StatPaint((percentage * 100).ToString()) + "% per turret on the ship";
    }
}
