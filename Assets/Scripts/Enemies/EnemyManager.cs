using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IManager
{
    [SerializeField] private EnemyType enemyType;
    private EnemyWiggler wiggler;
    private AttackController attackController;
    private int attackCooldown = 0;

    public void DestroyManager()
    {
        wiggler.OnWigglePeak -= TriggerAttack;
        Destroy(this);
    }

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

    public EnemyType GetEnemyType()
    {
        return enemyType;
    }
}
