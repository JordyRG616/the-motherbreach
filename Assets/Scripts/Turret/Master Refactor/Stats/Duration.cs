using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duration : TurretStat
{
    public override float Value { get => _value; protected set => _value = value; }

    protected override void SetValue(float value)
    {
        weapon.waitForDuration = new WaitForSeconds(value);
    }
}
