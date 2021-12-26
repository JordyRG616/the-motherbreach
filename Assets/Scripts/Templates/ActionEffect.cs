using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionEffect : MonoBehaviour
{
    [SerializeField] protected ParticleSystem shooter;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected WeaponClass weaponClass;
    protected GameObject target;
    [SerializeField] protected float initialDamage;
    [SerializeField] protected float initialRest;
    public Dictionary<Stat, float> StatSet {get; protected set;} = new Dictionary<Stat, float>();

    public delegate void Effect(HitManager hitManager);

    public Effect totalEffect;

    public virtual void Initiate()
    {        
        shooter.Stop();

        SetData();

        totalEffect += ApplyEffect;
    }

    public WeaponClass GetClass()
    {
        return weaponClass;
    }

    protected virtual void SetData()
    {
        StatSet.Add(Stat.Damage, initialDamage);
        StatSet.Add(Stat.Rest, initialRest);

        var col = shooter.collision;
        col.collidesWith = targetLayer;
    }

    public virtual void SetStat(Stat statName, float value)
    {
        if(StatSet.ContainsKey(statName))
        {
            StatSet[statName] = value;
        } 
    }

    public virtual void ReceiveTarget(GameObject parentTarget)
    {
        target = parentTarget;
    }

    public virtual void Shoot()
    {
        if(target != null && !shooter.isEmitting)
        {
            shooter.Play();
        }
    }

    public virtual void StopShooting()
    {
        shooter.Stop();
    }

    public virtual void RotateShoots(float angle)
    {
        // var main = shooter.main;
        // main.startRotation = angle * Mathf.Deg2Rad;
    }

    public virtual void RotateShoots()
    {
        // var parent = GetComponentInParent<Transform>();
        // float angle = - parent.rotation.eulerAngles.z;
        // var main = shooter.main;
        // main.startRotation = angle * Mathf.Deg2Rad;
    }

    public ParticleSystem GetShooterSystem()
    {
        return shooter;
    }

    public abstract void ApplyEffect(HitManager hitManager);

}