using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : ActionController
{
    // [SerializeField] private ActionEffect action;
    public bool Sleeping;
    
    public event EventHandler<EnemyEventArgs> OnDeath;

    void Start()
    {
        shooters.ForEach(x => x.Initiate());
        // action.Initiate();
    }

    public void SetTarget(GameObject target)
    {
        shooters.ForEach(x => x.ReceiveTarget(target));
        // action.ReceiveTarget(target);
    }

    public void Attack()
    {
        if(Sleeping) return;
        shooters.ForEach(x => x.Shoot());
        // action.Shoot();
    }

    public void Stop()
    {
        shooters.ForEach(x => x.StopShooting());
        // action.StopShooting();
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

    public void LevelUp(int targetLevel)
    {
        for(int i = 1; i <= targetLevel; i++)
        {
            foreach(ActionEffect shooter in shooters)
            {
                shooter.LevelUp(i);
            }
        }
    }
}
