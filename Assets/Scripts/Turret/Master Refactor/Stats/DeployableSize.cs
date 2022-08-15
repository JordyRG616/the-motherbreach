using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployableSize : Size
{
    public override float Value
    {
        get => _value;
        protected set => _value = value;
    }

    protected override void SetValue(float value)
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var main = shooter.main;
            main.startSize = value;
        }
    }

    public override string GetLiteralValue()
    {
        var percentage = _value / startingValue;
        percentage *= 100;
        return percentage.ToString("#.#") + "%";
    }

    public override string GetLiteralStartingValue()
    {
        return "100%";
    }
}
