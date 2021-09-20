using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEventArgs : EventArgs
{
    public EnemyHealthController healthController;

    public AttackController attackController;

    public EnemyEventArgs(EnemyHealthController enemyHealth)
    {
        healthController = enemyHealth;
    }

    public EnemyEventArgs(AttackController attack)
    {
        attackController = attack;
    }
}
