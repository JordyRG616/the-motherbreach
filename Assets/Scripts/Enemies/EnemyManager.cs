using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EnemyWiggler wiggler;
    private AttackController attackController;
    private int attackCooldown = 0;

    void Awake()
    {
        wiggler = GetComponent<EnemyWiggler>();
        attackController = GetComponent<AttackController>();

        wiggler.OnWigglePeak += TriggerAttack;
    }

    private void TriggerAttack(object sender, EventArgs e)
    {
        if(attackCooldown == attackController.cooldown)
        {
            attackController.Attack();
            attackCooldown = 0;
        } else
        {
            attackCooldown++;
        }
    }
}
