using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    
    public BaseEffectTemplate baseEffect {get; private set;}
    public ActionController actionController {get; private set;}
    public Dictionary<Stat, float> Stats {get; protected set;} = new Dictionary<Stat, float>();


    public void Initiate()
    {
        baseEffect = GetComponentInChildren<BaseEffectTemplate>();
        actionController = GetComponentInChildren<ActionController>();

        GetStats();
        
        var integrityManager = GetComponent<IntegrityManager>();
        integrityManager.Initiate(Stats[Stat.Health]);
    }

    private void GetStats()
    {
        Stats.Add(Stat.Cost, baseEffect.GetCost() + actionController.GetCost());
        Stats.Add(Stat.Health, actionController.GetHealth());

        var stats = actionController.GetShooters()[0].StatSet;

        foreach(Stat stat in stats.Keys)
        {
            Stats.Add(stat, stats[stat]);
        }

    }
    
}