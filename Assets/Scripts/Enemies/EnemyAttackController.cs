using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] private ActionEffect action;
    
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
        action.RotateShoots(transform.rotation.eulerAngles.z);
    }
    
}
