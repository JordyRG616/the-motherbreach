using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEventArgs : EventArgs
{
    public EnemyHealthController healthController;

    public EnemyAttackController attackController;

    public EnemyEventArgs(EnemyHealthController enemyHealth)
    {
        healthController = enemyHealth;
    }

    public EnemyEventArgs(EnemyAttackController attack)
    {
        attackController = attack;
    }
}
