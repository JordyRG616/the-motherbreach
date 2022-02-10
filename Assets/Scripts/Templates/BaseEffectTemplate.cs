using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffectTemplate : MonoBehaviour
{
    [SerializeField] protected BaseEffectTrigger trigger;
    [SerializeField] protected List<WeaponClass> targetedClasses;
    public bool targetStats;
    [SerializeField] protected List<Stat> targetedStats;
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

    public virtual void HandleLevelTrigger(object sender, LevelUpArgs e)
    {
        ApplyEffect();
    }

    public virtual void HandleHealthTrigger(object sender, EventArgs e)
    {
        ApplyEffect();
    }

    public abstract void ApplyEffect();

    public abstract string DescriptionText();
    
    public virtual string DescriptionText(out Keyword keyword, WeaponClass weaponClass = WeaponClass.Default)
    {
        keyword = this.keyword;
        if(weaponClass != WeaponClass.Default) return DescriptionTextByClass(weaponClass);
        else return DescriptionText();
    }

    public virtual string DescriptionTextByClass(WeaponClass weaponClass)
    {
        return DescriptionText();
    }
    
    public virtual string DescriptionTextByStat(Stat stat)
    {
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

    void OnDisable()
    {
        if(associatedController != null) gameManager.OnGameStateChange -= HandleEffectTrigger;
    }

    public bool StatIsTarget(Stat stat)
    {
        if(targetedStats.Contains(stat)) return true;
        else return false;
    }
}
