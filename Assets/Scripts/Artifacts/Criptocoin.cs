using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Criptocoin : Artifact
{
    [SerializeField] private int extraReward;

    public override string Description()
    {
        return "receive " + extraReward + " cash at the end of each wave";
    }

    protected override void Effect()
    {
        RewardManager.Main.EarnCash(extraReward);
    }
}
