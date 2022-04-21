using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NanoRegen : ShipAbility
{
    protected override void ActionEffect()
    {
        shipManager.turrets.ForEach(x => HealTurret(x));
    }

    private void HealTurret(TurretManager turret)
    {
        var heal = turret.integrityManager.GetMaxIntegrity();
        heal *= .1f;
        turret.integrityManager.UpdateHealth(heal);
    }
}
