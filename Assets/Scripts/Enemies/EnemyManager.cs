using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IManager
{
    private FormationManager owner;
    private EnemyAttackController attackController;
    private bool attacking;

    void Start()
    {
        if(transform.parent != null)
        {
            owner = GetComponentInParent<FormationManager>();
            owner.RegisterChildren(this);
        }

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
        if(attacking) return;
        attackController.Attack();
        attacking = true;
    }

    public void CeaseFire()
    {
        if(!attacking) return;
        attackController.Stop();
        attacking = false;
    }
}
