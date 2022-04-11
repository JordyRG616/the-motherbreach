using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ActionController : MonoBehaviour, ISavable
{
    public int weaponID;
    [SerializeField] protected List<ActionEffect> shooters;
    [SerializeField] protected float cost;
    [SerializeField] protected float health;
    protected float _health;

    public RestBarManager restBar;

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

    protected virtual void SetOnRest()
    {
        shooters.ForEach(x => x.SetToRest());
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

    public void SaveStats()
    {
        shooters.ForEach(x => x.RememberStatSet());
    }

    public void LoadStats()
    {
        shooters.ForEach(x => x.ResetStatSet());
    }

    protected virtual void FixedUpdate()
    {
        if(restBar == null) return;
        var value = shooters[0].GetRestPercentual();
        restBar.SetBarPercentual(value);
    }

    public Dictionary<string, byte[]> GetData()
    {
        var container = new Dictionary<string, byte[]>();
        container.Add("weaponID", BitConverter.GetBytes(weaponID));

        for(int i = 0; i < shooters.Count; i++)
        {
            var shooterData = shooters[i].GetData();

            foreach(string key in shooterData.Keys)
            {
                container.Add("shooter" + i + key, shooterData[key]);
            }
        }

        return container;
    }

    public void LoadData(SaveFile saveFile)
    {
        for (int i = 0; i < shooters.Count; i++)
        {
           foreach(Stat stat in shooters[i].StatSet.Keys)
           {

           } 
        }
    }

    public void LoadData(SaveFile saveFile, string rootId)
    {
        for (int i = 0; i < shooters.Count; i++)
        {
            var stats = shooters[i].StatSet.Keys.ToList();

            foreach(Stat stat in stats)
            {
                var value = BitConverter.ToSingle(saveFile.GetValue(rootId + "shooter" + i + stat));
                shooters[i].SetStat(stat, value);
            } 
        }
    }
}
