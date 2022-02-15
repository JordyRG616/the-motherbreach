using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : Artifact
{
    [Range(0, 1)] [SerializeField] private float percentage;

    public override string Description()
    {
        return "raises the health of the ship by " + percentage * 100 + "%";
    }

    protected override void Effect()
    {
        FindObjectOfType<ShipDamageController>().ModifyHealthByPercentage(percentage);
    }
}
