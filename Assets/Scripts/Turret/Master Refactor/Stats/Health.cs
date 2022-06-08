using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : TurretStat
{
    private IntegrityManager integrityManager;
    public override float Value { get => _value; protected set => _value = value; }

    public override void Initiate(ParticleSystem shooter, Weapon weapon)
    {
        integrityManager = GetComponentInParent<IntegrityManager>(true);
        base.Initiate(shooter, weapon);
        integrityManager.Initiate(Value);
    }

    protected override void SetValue(float value)
    {
        integrityManager.SetMaxIntegrity(value);
    }
}
