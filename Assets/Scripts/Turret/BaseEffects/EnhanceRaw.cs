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
    [SerializeField] private ParticleSystem vfx;
    [SerializeField] [FMODUnity.EventRef] private string sfx;

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

                    vfx.Play();
                    AudioManager.Main.RequestSFX(sfx);
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
        var value = (useFlatValue)? flatValue.ToString() : (percentage * 100).ToString() +"%" ;
        return "raises the " + StatColorHandler.StatPaint(container) + " of this turret by " + StatColorHandler.StatPaint(value);
    }

    public override string DescriptionTextByStat(Stat stat)
    {
        var value = (useFlatValue)? flatValue.ToString() : (percentage * 100).ToString() +"%" ;
        return "raises the " + StatColorHandler.StatPaint(stat.ToString().ToLower()) + " of this turret by " + StatColorHandler.StatPaint(value);
    }
}