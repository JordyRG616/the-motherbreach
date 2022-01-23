using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IManager
{
    private FormationManager owner;
    private EnemyAttackController attackController;

    void Start()
    {
        owner = GetComponentInParent<FormationManager>();
        owner.RegisterChildren(this);
        attackController = GetComponent<EnemyAttackController>();
        attackController.SetTarget(FindObjectOfType<ShipManager>().gameObject);
    }

    public void DestroyManager()
    {
        owner.RemoveEnemy(this);
        Destroy(this);
    }

    public void OpenFire()
    {
        attackController.Attack();
    }

    public void CeaseFire()
    {
        attackController.Stop();
    }
}
