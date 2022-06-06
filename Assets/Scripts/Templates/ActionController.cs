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
    [Header("Level Up")]
    [SerializeField] private List<LevelUpData> levelUpDatas;
    protected float _health;

    public RestBarManager restBar;

    protected List<TargetableComponent> enemiesInSight = new List<TargetableComponent>();
    public TargetableComponent target;
    private IntegrityManager integrityManager;
    protected GameManager gameManager;

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

    protected virtual void StopShooters(object sender, GameStateEventArgs e)
    {
        if (e.newState != GameState.OnWave) return;
        foreach (ActionEffect shooter in shooters)
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

        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += StopShooters;

        foreach(ActionEffect shooter in shooters)
        {
            shooter.Initiate();
        }
    }

    public void HandleLevelUp(object sender, LevelUpArgs e)
    {
        foreach(LevelUpData data in levelUpDatas)
        {
            if(data.level == e.toLevel) shooters.ForEach(x => data.ApplyLevelUp(x));
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<TargetableComponent>(out TargetableComponent enemy))
        {
            if(enemiesInSight.Contains(enemy)) enemiesInSight.Remove(enemy);
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

    protected virtual void OnDisable()
    {
        if (gameManager != null) gameManager.OnGameStateChange -= StopShooters;
    }

    public float GetCost()
    {
        return cost;
    }

    public float GetHealth()
    {
        return health;
    }

    public WeaponClass GetWeaponClass()
    {
        return shooters[0].weaponClass;
    }

    public void RaiseHealthByPercentage(float percentage)
    {
        health *= (1 + percentage);
        if(integrityManager == null) integrityManager = GetComponentInParent<IntegrityManager>();
        if(integrityManager == null) return;
        integrityManager.SetMaxIntegrity(health);
    }

    public void ReduceHealthByPercentage(float percentage)
    {
        health /= (1 + percentage);
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
        ReduceHealthByPercentage(.1f);
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
