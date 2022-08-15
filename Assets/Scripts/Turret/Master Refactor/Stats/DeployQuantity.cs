using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployQuantity : TurretStat
{
    public override float Value { get => _value; protected set => _value = value; }
    private DeployerWeapon deployer;


    private void Awake()
    {
        deployer = GetComponent<DeployerWeapon>();
    }

    protected override void SetValue(float value)
    {
        var _v = Mathf.RoundToInt(value);
        deployer.SetDeployQuantity(_v);
    }
}
