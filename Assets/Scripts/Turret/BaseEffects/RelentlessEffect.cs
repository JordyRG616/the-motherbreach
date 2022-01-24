using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

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
        string description = "reduces the " + StatColorHandler.RestPaint("rest") + " of this turret in " + StatColorHandler.StatPaint((percentage * 100).ToString()) + "%";
        return description;
    }
}
