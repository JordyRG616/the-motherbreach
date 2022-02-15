using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth;
    private EnemyVFXManager vfxManager;
    public float currentHealth {get; private set;}
    public float damageMultiplier = 1;

    public event EventHandler<EnemyEventArgs> OnDeath;


    void Start()
    {
        vfxManager = GetComponent<EnemyVFXManager>();
        UpdateHealth(maxHealth);

        RegisterListeners();
    }

    private void RegisterListeners()
    {
        var listeners = FindObjectsOfType<EnemyDeathEvent>();

        foreach(EnemyDeathEvent listener in listeners)
        {
            OnDeath += listener.ApplyEffect;
        }
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
        Destroy(gameObject);
    }
    
    public void UpdateHealth(float amount)
    {
        currentHealth += amount * damageMultiplier;

        if(amount < 0)
        {
            vfxManager.PlayHitEffect();
        } 
            
        if(currentHealth <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<EnemyManager>().CeaseFire();
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            vfxManager.StartCoroutine(vfxManager.LastBreath());
            OnDeath?.Invoke(this, new EnemyEventArgs(this));
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
