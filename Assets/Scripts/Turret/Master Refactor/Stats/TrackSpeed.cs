using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSpeed : TurretStat
{
    private HunterTargetSystem targetSystem;

    public override float Value 
    {
        get => _value;
        protected set => _value = value; 
    }


    public override void Initiate(ParticleSystem shooter, Weapon weapon)
    {
        base.Initiate(shooter, weapon);
        targetSystem = GetComponent<HunterTargetSystem>();
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
