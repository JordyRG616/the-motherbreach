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
    [field: SerializeField] public int upgradeCost { get; protected set; } = 1;
    public float increment;
    [field: SerializeField] public bool Initiated { get; protected set; }
    protected List<ParticleSystem> shooters = new List<ParticleSystem>();
    protected Weapon weapon;
    protected TurretManager manager;
    public bool overwritten;

    public delegate void IncrementDelegate(float amount);
    public IncrementDelegate incrementDelegate;

    public abstract float Value { get; protected set; } 
    protected float _value;

    protected abstract void SetValue(float value);

    public virtual void Initiate(ParticleSystem shooter, Weapon weapon)
    {
        if (Initiated) return;

        shooters.Add(shooter);
        this.weapon = weapon;

        Value = startingValue;
        SetValue(Value);

        RegisterIncrement();

        Initiated = true;
    }

    public void ReceiveShooter(ParticleSystem shooter)
    {
        shooters.Add(shooter);
    }

    public void RemoveShooter(ParticleSystem shooter)
    {
        shooters.Remove(shooter);
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
        if (!overwritten) return false;
        manager = GetComponentInParent<TurretManager>();
        if (manager == null) return false;
        return manager.upgradePoints >= upgradeCost;
    }

    private void SpendUpgradePoint(float amount)
    {
        manager.upgradePoints -= upgradeCost;
    }

    public void ApplyPercentage(float percentage)
    {
        Value *= 1 + percentage;
        SetValue(Value);
    }

    public void RemovePercentage(float percentage)
    {
        Value /= 1 + percentage;
        SetValue(Value);
    }

    public void ApplyFlat(float amount)
    {
        Value += amount;
        SetValue(Value);
    }

    public virtual string GetLiteralValue()
    {
        return Value.ToString("0.0");
    }

    public virtual string GetLiteralStartingValue()
    {
        return startingValue.ToString("0.0");
    }

    public void SetStatToValue(float value)
    {
        Value = value;
        SetValue(Value);
    }
}

