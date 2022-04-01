using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm : Artifact
{
    private RerrollButton RerrollButton;

    private int ogCost;

    public override string Description()
    {
        return "your first reroll each shop phase is free";
    }

    protected override void Effect()
    {
        RerrollButton = FindObjectOfType<RerrollButton>();
        ogCost = RerrollButton.rerrollCost;
        RerrollButton.rerrollCost = 0;
        RerrollButton.OnReroll += ResetCost;
    }

    private void ResetCost(object sender, EventArgs e)
    {
        RerrollButton.rerrollCost = ogCost;
        RerrollButton.OnReroll -= ResetCost;
    }
}
