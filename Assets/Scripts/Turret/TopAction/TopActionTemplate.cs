using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TopActionTemplate : MonoBehaviour
{
    [SerializeField] protected TargetType targetType{get;}
    [SerializeField] protected float cooldown;
    [SerializeField] protected int cost, hull;

    public abstract void ActivateAction(GameObject target);
    
    public int ReturnCost()
    {
        return cost;
    }

    public int ReturnHull()
    {
        return hull;
    }

    public float ReturnCooldown()
    {
        return cooldown;
    }
}
