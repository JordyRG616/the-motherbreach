using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth;
    private EnemyVFXManager vfxManager;
    private float currentHealth;

    public event EventHandler<EnemyEventArgs> OnDeath;
    public event EventHandler OnDamage;

    void Start()
    {
        vfxManager = GetComponent<EnemyVFXManager>();
        UpdateHealth(maxHealth);
    }

    public void DestroyDamageable()
    {
        foreach(IManager manager in GetComponents<IManager>())
        {
            manager.DestroyManager();
        }        
    }

    public void TriggerOnDeath()
    {
        vfxManager.StopAllCoroutines();
        DestroyDamageable();
        OnDeath?.Invoke(this, new EnemyEventArgs(this));
        Destroy(gameObject);
    }
    
    public void UpdateHealth(float amount)
    {
        currentHealth += amount;
        if(amount < 0)
        {
            vfxManager.PlayHitEffect();
            OnDamage?.Invoke(this, EventArgs.Empty);
        } 
            
        if(currentHealth <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<EnemyManager>().CeaseFire();
            vfxManager.StartCoroutine(vfxManager.LastBreath());
        }

        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        vfxManager.StartCoroutine(vfxManager.UpdateHealthBar(currentHealth, maxHealth));
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}
