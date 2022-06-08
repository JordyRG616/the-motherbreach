using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enrage : ShipAbility
{
    private Dictionary<ActionEffect, float> ogRests = new Dictionary<ActionEffect, float>();

    void Start()
    {
        GameManager.Main.OnGameStateChange += ResetRest;
    }

    private void ResetRest(object sender, GameStateEventArgs e)
    {
        if(e.newState != GameState.OnWave) return;

        foreach(ActionEffect shooter in ogRests.Keys)
        {
            shooter.SetStat(Stat.Rest, ogRests[shooter]);
        }
    }

    protected override void ActionEffect()
    {
        var rdm = Random.Range(0, shipManager.GetTurretCount());
        var turret = shipManager.turrets[rdm];

        //turret.actionController.GetShooters().ForEach(ReduceRest);
    }

    private void ReduceRest(ActionEffect shooter)
    {
        ogRests.Add(shooter, shooter.StatSet[Stat.Rest]);

        shooter.SetStat(Stat.Rest, 0);
    }
}
