using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffectTemplate : MonoBehaviour
{
    [SerializeField] protected BaseEffectTrigger trigger;
    [SerializeField] protected List<WeaponClass> targetedClasses;
    [SerializeField] protected float cost;
    [SerializeField] protected Keyword keyword;
    public bool previewable;

    protected ActionController associatedController;
    protected GameManager gameManager;
    protected TurretManager turretManager;


    public virtual void Initiate()
    {
        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += HandleEffectTrigger;

        turretManager = GetComponentInParent<TurretManager>();
    }

    private void HandleEffectTrigger(object sender, GameStateEventArgs e)
    {
        if(e.effectTrigger == trigger)
        {
            ApplyEffect();
        }
    }

    public virtual void HandleLevelEffect(object sender, LevelUpArgs e){}

    public abstract void ApplyEffect();

    public abstract string DescriptionText();
    public virtual string DescriptionText(out Keyword keyword)
    {
        keyword = this.keyword;
        return DescriptionText();
    }


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
