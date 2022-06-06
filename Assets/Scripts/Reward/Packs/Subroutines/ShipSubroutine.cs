using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipSubroutine : ScriptableObject
{
    [SerializeField] protected ShipAttribute targetAttribute;
    [SerializeField] protected float value;
    protected float cachedValue;
    [TextArea] public string description;
    protected bool active = false;

    protected ShipManager ship;

    public delegate void ShipSubroutineEffect(float value);
    protected ShipSubroutineEffect effect;

    public virtual void Initiate()
    {
        ship = ShipManager.Main;
        InitiateEffect();
    }

    protected virtual void InitiateEffect()
    {
        switch(targetAttribute)
        {
            case ShipAttribute.MaxHealth:
                effect = RaiseMaxHealth;
                break;
            case ShipAttribute.HealthRegen:
                effect = RaiseHealthRegen;
                break;
            case ShipAttribute.DamageReduction:
                effect = RaiseDamageReduction;
                break;
            case ShipAttribute.MovementSpeed:
                effect = RaiseMovementSpeed;
                break;
            case ShipAttribute.TurnSpeed:
                effect = RaiseTurnSpeed;
                break;
            case ShipAttribute.EnergyGain:
                effect = RaiseEnergyGain;
                break;
        }
    }

    protected abstract bool SubroutineCondition();

    public abstract string RequirementText();

    public void UpdateSubroutine()
    {
        if (SubroutineCondition() && active)
        {
            if (active)
            {
                Debug.Log("1");
                EndSubroutine();
                BeginSubroutine();
                return;
            }

            else
            {
                Debug.Log("2");
                BeginSubroutine();
                return;
            }
        }
        if(!SubroutineCondition() && active)
        {
        Debug.Log("3");
            EndSubroutine();
            return;
        }
    }

    protected virtual void BeginSubroutine()
    {
        Debug.Log("begin");
        cachedValue = value;
        effect?.Invoke(cachedValue);
        active = true;
    }

    protected virtual void EndSubroutine()
    {
        effect?.Invoke(-cachedValue);
        active = false;
    }

    protected virtual void RaiseMaxHealth(float percentage)
    {
        var controller = ship.GetComponent<ShipDamageController>();
        controller.ModifyHealthByPercentage(percentage);
    }

    protected virtual void RaiseHealthRegen(float amount)
    {
        var controller = ship.GetComponent<ShipDamageController>();
        controller.ModifyHealthRegen(amount);
    }

    protected virtual void RaiseDamageReduction(float amount)
    {
        var controller = ship.GetComponent<ShipDamageController>();
        controller.ModifyDamageReduction(amount);
    }

    protected virtual void RaiseMovementSpeed(float percentage)
    {
        var controller = FindObjectOfType<ShipController>();
        controller.ModifyMovementSpeedByPercentage(percentage);
    }

    protected virtual void RaiseTurnSpeed(float percentage)
    {
        var controller = FindObjectOfType<ShipController>();
        controller.ModifyTurnSpeedByPercentage(percentage);
    }

    protected virtual void RaiseEnergyGain(float percentage)
    {
        var controller = ship.GetComponent<ShipAbility>();
        controller.ModifyEnergyGainByPercentage(percentage);
    }
}
