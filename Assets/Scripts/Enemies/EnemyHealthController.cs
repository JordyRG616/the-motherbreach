using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth;
    private EnemyVFXManager vfxManager;
    [SerializeField] private float currentHealth;

    public event EventHandler<EnemyEventArgs> OnDeath;

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

    // private IEnumerator LastBreath()
    // {
    //     float step = 0;

    //     do
    //     {
    //         step += .01f;
    //         material.SetFloat("_death", step);
    //         yield return new WaitForSeconds(.01f);
    //     } while(step < 1);

    //     OnDeath?.Invoke(this, new EnemyEventArgs(this));

    //     Destroy(gameObject);
    // }

    public void UpdateHealth(float amount)
    {
        currentHealth += amount;
        if(currentHealth <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            vfxManager.StartCoroutine(vfxManager.LastBreath());
        }

        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        vfxManager.StartCoroutine(vfxManager.UpdateHealthBar(currentHealth, maxHealth));
    }

    // public void UpdateHealthBar()
    // {
    //     float percentual = Mathf.Min(1 - currentHealth / maxHealth, 1);

    //     material.SetFloat("_damagePercentual", percentual);
    // }
}
