using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurretStat : MonoBehaviour
{
    public float startingValue;
    protected ParticleSystem shooter;
    protected Weapon weapon;

    public abstract float Value { get; protected set; }
    protected float _value;

    protected abstract void SetValue(float value);

    public virtual void Initiate(ParticleSystem shooter, Weapon weapon)
    {
        this.shooter = shooter;
        this.weapon = weapon;

        Value = startingValue;
        SetValue(Value);
    }

    public void ApplyPercentage(float percentage)
    {
        Value *= 1 + percentage;
        SetValue(Value);
    }

    public void ApplyFlat(int amount)
    {
        Value += amount;
        SetValue(Value);
    }
}
