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
    [SerializeField] protected List<EnemyManager> enemiesInSight = new List<EnemyManager>();
    [SerializeField] protected EnemyManager target;
  
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

    public void Initiate()
    {
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
        if(other.TryGetComponent<EnemyManager>(out EnemyManager enemy))
        {
            enemiesInSight.Add(enemy);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyManager>(out EnemyManager enemy))
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

    public List<WeaponClass> GetClasses()
    {
        List<WeaponClass> container = new List<WeaponClass>();
        
        foreach(ActionEffect shooter in shooters)
        {
            if(!container.Contains(shooter.GetClass()))
            {
                container.Add(shooter.GetClass());
            }
        }
        
        return container;
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
    }
}
