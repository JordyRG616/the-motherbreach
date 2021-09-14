using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleController : MonoBehaviour
{
    [SerializeField] private List<EnemyWiggler> leftWigglers;
    [SerializeField] private List<EnemyWiggler> rightWigglers;
    [SerializeField] private List<EnemyWiggler> upWigglers;
    [SerializeField] private List<EnemyWiggler> downWigglers;

    void Start()
    {
        ActivateWigglers();
    }

    private void ActivateWigglers()
    {
        foreach(EnemyWiggler enemy in leftWigglers)
        {
            enemy.StartWiggling(Vector3.left);
        }

        foreach(EnemyWiggler enemy in rightWigglers)
        {
            enemy.StartWiggling(Vector3.right);
        }
        
        foreach(EnemyWiggler enemy in upWigglers)
        {
            enemy.StartWiggling(Vector3.up);
        }
        
        foreach(EnemyWiggler enemy in downWigglers)
        {
            enemy.StartWiggling(Vector3.down);
        }
    }
}
