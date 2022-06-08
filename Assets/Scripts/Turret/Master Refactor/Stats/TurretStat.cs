using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurretStat : MonoBehaviour
{
    private enum IncrementType { Flat, Percentage}

    public int sortingIndex;
    public string publicName;
    public float startingValue;
    public string statDescription;
    [Header("Increment Information")]
    [SerializeField] private IncrementType incrementType;
    public float increment;
    public bool Initiated { get; protected set; }
    protected ParticleSystem shooter;
    protected Weapon weapon;
    protected TurretManager manager;

    public delegate void IncrementDelegate(float amount);
    public IncrementDelegate incrementDelegate;

    public abstract float Value { get; protected set; }
    protected float _value;

    protected abstract void SetValue(float value);

    public virtual void Initiate(ParticleSystem shooter, Weapon weapon)
    {
        if (Initiated) return;

        this.shooter = shooter;
        this.weapon = weapon;
        manager = GetComponentInParent<TurretManager>();

        Value = startingValue;
        SetValue(Value);

        RegisterIncrement();

        Initiated = true;
    }

    private void RegisterIncrement()
    {
        incrementDelegate += SpendUpgradePoint;
        switch (incrementType)
        {
            case IncrementType.Flat:
                incrementDelegate += ApplyFlat;
                break;
            case IncrementType.Percentage:
                incrementDelegate += ApplyPercentage;
                break;
        }
    }

    public bool CanUpgrade()
    {
        if (manager == null) return false;
        return manager.upgradePoints > 0;
    }

    private void SpendUpgradePoint(float amount)
    {
        manager.upgradePoints--;
    }

    public void ApplyPercentage(float percentage)
    {
        Value *= 1 + percentage;
        SetValue(Value);
    }

    public void ApplyFlat(float amount)
    {
        Value += amount;
        SetValue(Value);
    }

    public virtual string GetLiteralValue()
    {
        return Value.ToString();
    }

    public virtual string GetLiteralStartingValue()
    {
        return startingValue.ToString();
    }
}
