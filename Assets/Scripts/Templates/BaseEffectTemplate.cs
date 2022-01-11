using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffectTemplate : MonoBehaviour
{
    [SerializeField] protected BaseEffectTrigger trigger;
    [SerializeField] protected List<WeaponClass> targetedClasses;
    [SerializeField] protected float cost;
    protected ActionController associatedController;
    protected GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += HandleEffectTrigger;

    }

    private void HandleEffectTrigger(object sender, GameStateEventArgs e)
    {
        if(e.effectTrigger == trigger)
        {
            ApplyEffect();
        }
    }

    public abstract void ApplyEffect();

    public abstract string DescriptionText();

    public void ReceiveWeapon(ActionController weapon)
    {
        associatedController = weapon;
    }

    public BaseEffectTrigger GetTrigger()
    {
        return trigger;
    }

    public List<WeaponClass> GetWeaponClasses()
    {
        return targetedClasses;
    }

    public float GetCost()
    {
        return cost;
    }

}
