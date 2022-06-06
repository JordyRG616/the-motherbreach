using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeployerActionEffect : ActionEffect
{
    [SerializeField] protected float capacity;
    [SerializeField] protected DeployablePool pool;


    public override void Initiate()
    {
        base.Initiate();
        pool.Initiate();
    }

    public override void Shoot()
    {
        if(pool.GetDeployedObjectCount() == StatSet[Stat.Capacity]) return;
        var deployable = pool.RequestDeployable();
        deployable.transform.position = transform.position;
        deployable.Born();
        // deployable.transform.SetParent(transform);
    }
    
}
