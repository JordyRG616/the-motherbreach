using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class IncendiaryEffect : BaseEffectTemplate
{
    public override void ApplyEffect()
    {
        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            shooter.totalEffect += AddBurn;
        }
    }

    public void AddBurn(HitManager hitManager)
    {
        ApplyStatusEffect<Acid>(hitManager, 2f, new float[] {associatedController.GetShooters()[0].StatSet[Stat.Damage] / 10, .1f});
    }


    public override string DescriptionText()
    {
        string description = "on hit, this turret applies " + KeywordHandler.KeywordPaint(Keyword.Acid);
        return description;
    }
}
