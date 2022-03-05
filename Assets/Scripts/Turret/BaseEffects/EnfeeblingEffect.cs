using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class EnfeeblingEffect : BaseEffectTemplate
{
    [Range(0, 1)] [SerializeField] private float percentage;
    public override void ApplyEffect()
    {
        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            shooter.totalEffect += AddWeaken;
        }
    }

    public void AddWeaken(HitManager hitManager)
    {
        ApplyStatusEffect<Weaken>(hitManager, 5f, new float[] {percentage});
    }


    public override string DescriptionText()
    {
        string description = "on hit, this turret applies " + KeywordHandler.KeywordPaint(Keyword.Weaken);
        return description;
    }
}
