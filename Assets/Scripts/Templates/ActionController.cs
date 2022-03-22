using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ActionController : MonoBehaviour
{
    [SerializeField] protected List<ActionEffect> shooters;
    [SerializeField] protected float cost;
    [SerializeField] protected float health;
    protected float _health;

    protected List<TargetableComponent> enemiesInSight = new List<TargetableComponent>();
    [HideInInspector] public TargetableComponent target;
    private IntegrityManager integrityManager;

    public abstract void Activate();

    protected abstract IEnumerator ManageActivation();

    protected virtual void StopShooters()
    {
        foreach(ActionEffect shooter in shooters)
        {
            shooter.StopShooting();
            shooter.ReceiveTarget(null);
        }
    }

    public virtual void Initiate()
    {
        _health = health;

        foreach(ActionEffect shooter in shooters)
        {
            shooter.Initiate();
        }
    }

    public void HandleLevelUp(object sender, LevelUpArgs e)
    {
        foreach(ActionEffect shooter in shooters)
        {
            shooter.LevelUp(e.toLevel);
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<TargetableComponent>(out TargetableComponent enemy))
        {
            enemiesInSight.Add(enemy);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if(other.TryGetComponent<TargetableComponent>(out TargetableComponent enemy))
        {
            if(enemiesInSight.Contains(enemy))
            {
                enemiesInSight.Remove(enemy);
                
            }
        }
    }

    public List<ActionEffect> GetShooters()
    {
        return shooters;
    }

    public void Reset()
    {
        health = _health;

        foreach(ActionEffect shooter in shooters)
        {
            shooter.StatSet.Clear();
            shooter.SetData();
        }
    }

    public float GetCost()
    {
        return cost;
    }

    public float GetHealth()
    {
        return health;
    }

    public void RaiseHealthByPercentage(float percentage)
    {
        health *= (1 + percentage);
        if(integrityManager == null) integrityManager = GetComponentInParent<IntegrityManager>();
        if(integrityManager == null) return;
        integrityManager.SetMaxIntegrity(health);
    }

    public List<Stat> GetStatsOnShooters()
    {
        var container = new List<Stat>();

        foreach(ActionEffect shooter in shooters)
        {
            foreach(Stat stat in shooter.StatSet.Keys)
            {
                if(!container.Contains(stat))
                {
                    container.Add(stat);
                }
            }
        }

        return container;
    }
}
