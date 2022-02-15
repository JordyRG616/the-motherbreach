using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycler : Artifact
{
    [SerializeField] private float amount;
    public override string Description()
    {
        return "receive " + amount + " extra cash when you sell a turret";
    }

    protected override void Effect()
    {
        RewardManager.Main.EarnCash(amount);
    }
}
