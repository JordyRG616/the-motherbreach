using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptainAmelie : Pilot
{
    [SerializeField] [Range(0, 1)] private float percentage;

    protected override void HandleBuildEffect(object sender, BuildEventArgs e)
    {
        //var shooters = e.buildedTurret.actionController.GetShooters();
        //shooters.ForEach(x => RaiseDamage(x));
    }
    
    public override string AbilityDescription()
    {
        return "When a turret is build,  raise it's damage by " + (percentage * 100) + "%";
    }

    private void RaiseDamage(ActionEffect shooter)
    {
        var damage = shooter.StatSet[Stat.Damage];
        damage *= 1 + percentage;
        shooter.SetStat(Stat.Damage, damage);
    }

    protected override void Effect()
    {
        
    }

}
