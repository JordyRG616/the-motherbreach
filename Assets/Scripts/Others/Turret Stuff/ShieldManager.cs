using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
    [SerializeField] [FMODUnity.EventRef] private string onHitSFX;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void DestroyDamageable()
    {
        Destroy(gameObject);
    }

    public void UpdateHealth(float amount)
    {
        currentHealth += amount;
        if(amount < 0)
        {
            AudioManager.Main.RequestSFX(onHitSFX);
        }
        if(currentHealth <= 0) DestroyDamageable();
    }

    public void UpdateHealthBar()
    {
    }

    
}
