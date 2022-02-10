using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelfDestruction : MonoBehaviour
{
    private EnemyHealthController healthInterface;

    void Awake()
    {
        healthInterface = GetComponent<EnemyHealthController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.TryGetComponent<HitManager>(out var hitManager)) return;
        healthInterface.UpdateHealth(-healthInterface.currentHealth);
    }
}
