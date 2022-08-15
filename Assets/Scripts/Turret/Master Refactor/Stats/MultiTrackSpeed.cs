using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTrackSpeed : TurretStat
{
    private MultiTargetSystem targetSystem;

    public override float Value
    {
        get => _value;
        protected set => _value = value;
    }


    public override void Initiate(ParticleSystem shooter, Weapon weapon)
    {
        base.Initiate(shooter, weapon);
        targetSystem = GetComponent<MultiTargetSystem>();
    }

    protected override void SetValue(float value)
    {
        targetSystem.SetTurnSpeed(value);
    }

    public override string GetLiteralValue()
    {
        var dif = startingValue = _value;
        return (startingValue + dif).ToString("#.#");
    }
}
