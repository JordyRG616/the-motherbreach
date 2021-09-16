using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionEffect : MonoBehaviour
{
    [SerializeField] protected ParticleSystem shooter;
    [SerializeField] protected ActionData data;

    protected virtual void Awake()
    {
        shooter.Stop();

        SetActionData();

        shooter.Play();
    }

    protected void SetActionData()
    {
        var main = shooter.main;
        main.startSpeed = data.bulletSpeed;
        main.startLifetime = data.bulletRange;
        main.duration = data.fireRate;
        main.startSize = data.bulletSize;
    }

    public abstract void Shoot();

    public abstract void ApplyEffect(HitManager hitManager);

    public ActionData GetData()
    {
        return data;
    }
    
    
}

[System.Serializable]
public struct ActionData
{
    public TargetType target;
    public float bulletSpeed;
    public float bulletRange;
    public float bulletSize;
    public float fireRate;
    public float bulletDamage;
}