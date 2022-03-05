using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathEvent : MonoBehaviour
{
    public delegate void TriggerEffect();
    public TriggerEffect effect;
    [HideInInspector] public GameObject killer;
    [HideInInspector] public GameObject victim;

    public void ApplyEffect(object sender, EnemyEventArgs e)
    {
        killer = e.healthController.GetComponent<HitManager>().lastAttacker;
        victim = e.healthController.gameObject;
        effect?.Invoke();
    }
}
