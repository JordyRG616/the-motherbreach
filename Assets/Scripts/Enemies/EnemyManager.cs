using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IManager
{
    private FormationManager owner;

    void Start()
    {
        owner = GetComponentInParent<FormationManager>();
    }

    public void DestroyManager()
    {
        owner.RemoveEnemy(this);
        Destroy(this);
    }

}
