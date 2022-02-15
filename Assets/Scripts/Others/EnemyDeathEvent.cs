using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathEvent : MonoBehaviour
{
    public delegate void TriggerEffect();
    public TriggerEffect effect;
    public GameObject killer;

    public void ApplyEffect(object sender, EnemyEventArgs e)
    {
        killer = e.healthController.GetComponent<HitManager>().lastAttacker;
        effect?.Invoke();
    }
}
