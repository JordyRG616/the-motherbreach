using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] private ActionEffect action;
    
    public event EventHandler<EnemyEventArgs> OnDeath;


    private void GetTarget()
    {
        var turrets = FindObjectsOfType<TurretManager>();
        turrets.OrderBy(x => Vector3.Distance(transform.position, x.transform.position));
        var target = turrets.FirstOrDefault();
        action.ReceiveTarget(target.gameObject);
    }

    public void Attack()
    {
        GetTarget();
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
