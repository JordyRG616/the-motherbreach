using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelfDestruction : MonoBehaviour
{
    private EnemyHealthController healthInterface;
    private EnemyActionEffect action;

    void Awake()
    {
        healthInterface = GetComponent<EnemyHealthController>();
        action = GetComponent<EnemyActionEffect>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.TryGetComponent<HitManager>(out var hitManager)) return;
        // hitManager.HealthInterface.UpdateHealth(action.StatSet[Stat.Damage]);
        healthInterface.UpdateHealth(-healthInterface.currentHealth);
    }
}
