using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployableLifetime : Duration
{
    private DeployerWeapon deployer;

    private void Awake()
    {
        deployer = GetComponent<DeployerWeapon>();
    }

    protected override void SetValue(float value)
    {
        deployer.GetModel().SetLifetime(value);
        deployer.GetInstantiatedDeployables().ForEach(x => x.SetLifetime(value));
    }
}
