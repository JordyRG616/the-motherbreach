using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegrityManager : MonoBehaviour, IDamageable, IManager
{
    private float maxIntegrity;
    [SerializeField] private SpriteRenderer barRenderer;
    private float currentIntegrity;

    public void Initiate(float maxHealth)
    {
        this.maxIntegrity = maxHealth;
        currentIntegrity = maxIntegrity;
    }

    public void DestroyDamageable()
    {
        foreach(IManager manager in GetComponents<IManager>())
        {
            manager.DestroyManager();
        }

        GetComponentInChildren<TurretVFXManager>().StartCoroutine(GetComponentInChildren<TurretVFXManager>().Die(gameObject));

        // Destroy(gameObject);
    }

    public void UpdateHealth(float amount)
    {
        currentIntegrity += amount;
        if(amount < 0)
        {
            GetComponentInChildren<TurretVFXManager>().StartCoroutine(GetComponentInChildren<TurretVFXManager>().TakeDamage());
        }
        if(currentIntegrity <= 0)
        {
            DestroyDamageable();
        }
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        float percentual = currentIntegrity / maxIntegrity;

        barRenderer.material.SetFloat("_healthPercentual", percentual);
    }

    public void DestroyManager()
    {

    }

    public void RaiseMaxIntegrityByAmount(float amount)
    {
        maxIntegrity += amount;
    }

    public void RaiseMaxIntegrityByPercentage(float percentage)
    {
        maxIntegrity *= (1 + percentage);
    }
}
