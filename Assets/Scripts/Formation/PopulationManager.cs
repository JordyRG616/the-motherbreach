using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour, IManager
{
    private List<EnemyHealthController> enemies = new List<EnemyHealthController>();
    private int populationLow;


    public event EventHandler OnPopulationEmpty;
    public event EventHandler OnPopulationLow;



    [ContextMenu("Teste")]
    public void test()
    {
        foreach(EnemyHealthController enemy in enemies)
        {
            enemy.UpdateHealth(-100);
        }
    }

    public void RegisterToEvent(WaveManager waveManager)
    {
        populationLow = Mathf.CeilToInt(enemies.Count / 3f);
        OnPopulationLow += waveManager.ResetInstantiator;
    }

    public void RegisterEnemy(EnemyHealthController enemy)
    {
        enemies.Add(enemy);
        enemy.OnDeath += RemoveEnemy;
    }

    public void DestroyManager()
    {
        Destroy(this);
    }

    public void RemoveEnemy(object sender, EnemyEventArgs e)
    {
        e.healthController.OnDeath -= RemoveEnemy;
        enemies.Remove(e.healthController);
        if(enemies.Count == 0)
        {
            OnPopulationEmpty?.Invoke(this, EventArgs.Empty);
        }
        else if(enemies.Count == populationLow)
        {
            OnPopulationLow?.Invoke(this, EventArgs.Empty);
        }    
    }
}
