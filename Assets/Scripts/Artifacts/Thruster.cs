using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : Artifact
{
    [Range(0, 1)] [SerializeField] private float percentage;

    public override string Description()
    {
        return "raises the speed of the ship by " + percentage * 100 + "%";
    }

    protected override void Effect()
    {
        FindObjectOfType<ShipController>().ModifySpeedByPercentage(percentage);
    }
}
