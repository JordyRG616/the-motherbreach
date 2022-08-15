using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpeed_RandomBetweenConstants : BulletSpeed
{
    public override float Value { get => _value; protected set => _value = value; }

    protected override void SetValue(float value)
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var main = shooter.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(-value, value);
        }
    }

    public override string GetLiteralValue()
    {
        return Value.ToString();
    }

    public override string GetLiteralStartingValue()
    {
        return startingValue.ToString();
    }
}
