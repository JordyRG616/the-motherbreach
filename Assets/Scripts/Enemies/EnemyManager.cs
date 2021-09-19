using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IManager
{
    [SerializeField] private EnemyType enemyType;
    private EnemyWiggler wiggler;

    public void DestroyManager()
    {
        Destroy(this);
    }

    void Awake()
    {
        wiggler = GetComponent<EnemyWiggler>();
    }

    public EnemyType GetEnemyType()
    {
        return enemyType;
    }
}
