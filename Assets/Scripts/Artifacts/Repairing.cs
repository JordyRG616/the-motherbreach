using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repairing : Artifact
{
    [SerializeField] [Range(0, 1)] private float percentage;
    private EnemyDeathEvent deathEvent;
    private ShipManager shipManager;

    public override void Initialize()
    {
        base.Initialize();

        shipManager = ShipManager.Main;
    }

    public override string Description()
    {
        return "heals every turret on the ship for " + percentage * 100 + "% of the defeated enemy maximum health";
    }

    protected override void Effect()
    {
        var heal = deathEvent.victim.GetComponent<EnemyHealthController>().GetMaxHealth() * percentage;

        foreach(TurretManager turret in shipManager.turrets)
        {
            turret.GetComponent<IntegrityManager>().UpdateHealth(heal);
        }
    }
}
