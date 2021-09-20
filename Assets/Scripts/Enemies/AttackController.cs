using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private ActionEffect action;
    
    public event EventHandler<EnemyEventArgs> OnDeath;

    public void Attack()
    {
        if(action)
        {
            action.RotateShoots();
            action.Burst();
        } else
        {
            OnDeath?.Invoke(this, new EnemyEventArgs(this));
        }
    }
    
}
