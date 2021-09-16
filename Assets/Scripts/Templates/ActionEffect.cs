using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionEffect : MonoBehaviour
{
    protected ParticleSystem shooter;
    [SerializeField] protected ActionData data;

    protected virtual void Awake()
    {
        shooter = GetComponent<ParticleSystem>();
        
        shooter.Stop();

        SetActionData();
    }

    protected void SetActionData()
    {
        var main = shooter.main;
        main.startSpeed = data.bulletSpeed;
        main.startLifetime = data.bulletRange;
        main.duration = data.fireRate;
        main.startSize = data.bulletSize;
    }

    public virtual void Shoot()
    {
        // if(!shooter.isPlaying)
        // {
            shooter.Play();
        // }
    }

    public virtual void StopShooting()
    {
        if(shooter.isPlaying)
        {
            shooter.Stop();
        }
    }

    public virtual void RotateShoots(float angle)
    {
        var main = shooter.main;
        main.startRotation = angle * Mathf.Deg2Rad;
    }

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