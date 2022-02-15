using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : Artifact
{
    public override string Description()
    {
        return "upgrade each turret in the ship by one level";
    }

    protected override void Effect()
    {
        var turrets = ShipManager.Main.turrets;

        foreach(TurretManager turret in turrets)
        {
            turret.LevelUp();
        }
    }
}
