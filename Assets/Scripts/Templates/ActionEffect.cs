using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionEffect : MonoBehaviour
{
    protected ParticleSystem shooter;
    protected EnemyManager target;
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
        main.startSpeed = data.speed;
        main.startLifetime = data.range;
        main.duration = data.cooldown;
        main.startSize = data.bulletSize;

        var coll = shooter.collision;
        coll.collidesWith = data.targetLayer;
    }

    public virtual void ReceiveTarget(EnemyManager parentTarget)
    {
        target = parentTarget;
    }

    public virtual void Shoot()
    {
        if(target != null)
        {
            shooter.Play();
        }
    }

    public virtual void Burst()
    {
        shooter.Play();
        Invoke("StopShooting", .1f);
    }

    public virtual void StopShooting()
    {
        shooter.Stop();
    }

    public virtual void RotateShoots(float angle)
    {
        var main = shooter.main;
        main.startRotation = angle * Mathf.Deg2Rad;
    }

    public virtual void RotateShoots()
    {
        var parent = GetComponentInParent<Transform>();
        float angle = - parent.rotation.eulerAngles.z;
        var main = shooter.main;
        main.startRotation = angle * Mathf.Deg2Rad;
    }

    public abstract void ApplyEffect(HitManager hitManager);

    public ActionData GetData()
    {
        return data;
    }

}