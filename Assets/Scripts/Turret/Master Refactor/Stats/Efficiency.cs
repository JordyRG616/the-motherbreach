using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Efficiency : TurretStat
{
    public override float Value 
    { 
        get => _value; 
        protected set
        {
            if (value > 1) _value = 1;
            else if (value < 0) _value = 0;
            else _value = value;
        }
    }

    protected override void SetValue(float value)
    {
    }
}
