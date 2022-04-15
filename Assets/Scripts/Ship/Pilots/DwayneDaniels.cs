using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwayneDaniels : Pilot
{
    [SerializeField] private int threshold;
    private int killCount;
    private RewardManager rewardManager;

    public override void Initialize()
    {
        base.Initialize();

        rewardManager = RewardManager.Main;
    }

    public override string AbilityDescription()
    {
        return "For every " + threshold + " enemies defeated in the same wave, gain 1$.";
    }

    protected override void Effect()
    {
        killCount++;

        if(killCount == threshold)
        {
            rewardManager.EarnCash(1);
            killCount = 0;
        }
    }
}
