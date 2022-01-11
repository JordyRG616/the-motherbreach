using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelentlessEffect : BaseEffectTemplate
{
    [SerializeField] [Range(0, 1)] private float percentage;

    public override void ApplyEffect()
    {
        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            float ogRest = shooter.StatSet[Stat.Rest];
            shooter.SetStat(Stat.Rest, ogRest * percentage);
        }
    }

    public override string DescriptionText()
    {
        string description = "";
        return description;
    }
}
