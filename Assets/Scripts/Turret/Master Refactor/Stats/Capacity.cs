using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capacity : TurretStat
{
    public override float Value { get => _value; protected set => _value = value; }


    protected override void SetValue(float value)
    {
    }
}