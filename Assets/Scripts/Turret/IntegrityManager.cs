using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegrityManager : MonoBehaviour, IDamageable, IManager
{
    private float maxIntegrity;
    [SerializeField] private SpriteRenderer barRenderer;
    private float currentIntegrity
    {
        get
        {
            return _cIntegrity;
        }
        set
        {
            _cIntegrity = Mathf.FloorToInt(value);
        }
    }
    private float _cIntegrity;
    private VFXManager vfxManager;
    [SerializeField] [FMODUnity.EventRef] private string onHitSFX;

    public void Initiate(float maxHealth)
    {
        this.maxIntegrity = maxHealth;
        currentIntegrity = maxIntegrity;
        vfxManager = GetComponentInChildren<TurretVFXManager>();
    }

    public void DestroyDamageable()
    {
        foreach(IManager manager in GetComponents<IManager>())
        {
            manager.DestroyManager();
        }

        vfxManager.StartCoroutine(GetComponentInChildren<TurretVFXManager>().Die(gameObject));

        // Destroy(gameObject);
    }

    public void SellTurret()
    {
        foreach (IManager manager in GetComponents<IManager>())
        {
            manager.DestroyManager();
        }

        Destroy(gameObject);
    }

    public void UpdateHealth(float amount)
    {
        currentIntegrity += amount;
        if(amount < 0)
        {
            AudioManager.Main.RequestSFX(onHitSFX);
            vfxManager.StartCoroutine(GetComponentInChildren<TurretVFXManager>().TakeDamage());
        }
        if(currentIntegrity > maxIntegrity)
        {
            currentIntegrity = maxIntegrity;
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

    public void HealToFull()
    {
        currentIntegrity = maxIntegrity;
        UpdateHealthBar();
    }

    public void DestroyManager()
    {

    }

    public void SetMaxIntegrity(float value)
    {
        maxIntegrity = value;
        HealToFull();
    }

    public void RaiseMaxIntegrityByAmount(float amount)
    {
        maxIntegrity += amount;
        HealToFull();
    }

    public void RaiseMaxIntegrityByPercentage(float percentage)
    {
        maxIntegrity *= (1 + percentage);
        HealToFull();
    }

    public float GetCurrentIntegrity()
    {
        return currentIntegrity;
    }

    public float GetMaxIntegrity()
    {
        return maxIntegrity;
    }

    public bool IsAtMaxIntegrity()
    {
        return Mathf.Approximately(maxIntegrity, currentIntegrity);
    }
}
