using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FormationManager : MonoBehaviour
{
    public event EventHandler OnFormationDefeat;
    public List<EnemyManager> Children {get; private set;} = new List<EnemyManager>();
    public int formationLevel;

    public virtual void Update()
    {
        if(Children.Count == 0)
        {
            OnFormationDefeat?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }

    internal void RegisterChildren(EnemyManager enemyManager)
    {
        Children.Add(enemyManager);
    }

    public void RemoveEnemy(EnemyManager enemy)
    {
        Children.Remove(enemy);
    }

    public void Terminate()
    {
        foreach(EnemyManager child in Children)
        {
            var health = child.GetComponent<EnemyHealthController>();
            health.UpdateHealth(-health.currentHealth);
        }
    }
}
