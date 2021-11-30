using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ActionController : MonoBehaviour
{
    [SerializeField] protected List<ActionEffect> shooters;
    protected List<EnemyManager> enemiesInSight = new List<EnemyManager>();
    protected EnemyManager target;
  
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
}
