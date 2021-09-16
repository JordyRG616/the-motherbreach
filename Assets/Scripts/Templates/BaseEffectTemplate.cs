using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffectTemplate : MonoBehaviour
{
    [SerializeField] protected int cost, hull;

    public abstract void ActivateMainEffect(TurretManager turretManager);
    public abstract void ActivateLocalEffect();

    public int ReturnCost()
    {
        return cost;
    }

    public int ReturnHull()
    {
        return hull;
    }
}
