using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    private TurretStats stats;
    private BaseEffectTemplate baseEffect;
    private TopActionTemplate topAction;
    private ActionController actionController;

    void Awake()
    {
        baseEffect = GetComponentInChildren<BaseEffectTemplate>();
        topAction = GetComponentInChildren<TopActionTemplate>();
        actionController = GetComponentInChildren<ActionController>();

        SetStats();

        baseEffect.ActivateMainEffect(this);
        
        actionController.Initialize(topAction, stats.cooldown);
    }   

    private void SetStats()
    {
        stats.cost = baseEffect.ReturnCost() + topAction.ReturnCost();
        stats.hull = baseEffect.ReturnHull() + topAction.ReturnHull();
        stats.cooldown = topAction.ReturnCooldown();
    }
    
    public void UpdateHull(float amount)
    {
        stats.hull += amount;
    }

    public void UpdateCooldown(float amount)
    {
        stats.cooldown += amount;
    }
}

public struct TurretStats
{
    public int cost;
    public float hull;
    public float cooldown;
}