using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IManager
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Color> levelColors;
    private FormationManager owner;
    public EnemyAttackController attackController {get; private set;}
    private EnemyHealthController healthController;
    private bool attacking;
    public int level {get; private set;}

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
        if(level >= 5 || waveLevel <= 0) return;
        level = waveLevel;
        attackController.LevelUp(waveLevel);
        healthController.RaiseHealthByPercentage(.1f * waveLevel);
        spriteRenderer.color = levelColors[waveLevel - 1];
    }
}
