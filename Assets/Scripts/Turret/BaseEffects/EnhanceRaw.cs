using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class EnhanceRaw : BaseEffectTemplate
{
    [SerializeField] [Range(0, 1)] private float percentage;
    [SerializeField] private bool useFlatValue;
    [SerializeField] private int flatValue;

    public override void ApplyEffect()
    {
        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            foreach(Stat stat in targetedStats)
            {
                if(shooter.StatSet.ContainsKey(stat))
                {
                    var value = shooter.StatSet[stat];
                    if(useFlatValue) value += flatValue;
                    else   value *= 1 + (percentage);
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
        var value = (useFlatValue)? flatValue.ToString() : (percentage * 100).ToString() +"%" ;
        return "raises " + container + " by " + StatColorHandler.StatPaint(value);
    }

    public override string DescriptionTextByStat(Stat stat)
    {
        var value = (useFlatValue)? flatValue.ToString() : (percentage * 100).ToString() +"%" ;
        return "raises the " + StatColorHandler.StatPaint(stat.ToString().ToLower()) + " by " + StatColorHandler.StatPaint(value);
    }
}
