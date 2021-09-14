using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorBE : BaseEffectTemplate
{
    [SerializeField] private float potency;

    public override void ActivateLocalEffect()
    {
        throw new System.NotImplementedException("Bônus local ainda não implementado");
    }

    public override void ActivateMainEffect(TurretManager turretManager)
    {
        turretManager.UpdateCooldown(-(potency * 5));
    }

}
