using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : ActionController
{
    [SerializeField] private ActionEffect action;
    public bool Sleeping;
    
    public event EventHandler<EnemyEventArgs> OnDeath;

    void Start()
    {
        action.Initiate();
    }

    public void SetTarget(GameObject target)
    {
        action.ReceiveTarget(target);
    }

    public void Attack()
    {
        action.Shoot();
    }

    public void Stop()
    {
        action.StopShooting();
    }

    void Update()
    {
        if(Sleeping) Stop();
    }

    public override void Activate()
    {
    }

    protected override IEnumerator ManageActivation()
    {
        yield return null;
    }
}
