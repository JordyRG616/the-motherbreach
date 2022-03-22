using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IManager
{
    private FormationManager owner;
    private EnemyAttackController attackController;
    private EnemyHealthController healthController;
    private bool attacking;
    private int level;

    void Start()
    {
        if(transform.parent != null)
        {
            owner = GetComponentInParent<FormationManager>();
            owner.RegisterChildren(this);
        }

        attackController = GetComponent<EnemyAttackController>();
        attackController.SetTarget(FindObjectOfType<ShipManager>().gameObject);

        healthController = GetComponent<EnemyHealthController>();

        AdjustLevel(owner.formationLevel);
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

    public void AdjustLevel(int waveLevel)
    {
        if(level == 5 || waveLevel == 0) return;
        level = waveLevel;
        attackController.LevelUp(waveLevel);
        healthController.RaiseHealthByPercentage(.1f * waveLevel);
    }
}
