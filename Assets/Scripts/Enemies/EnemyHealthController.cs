using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth;
    private float currentHealth;
    private Material material;

    public event EventHandler<EnemyEventArgs> OnDeath;

    void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        UpdateHealth(maxHealth);
    }

    public void DestroyDamageable()
    {
        foreach(IManager manager in GetComponents<IManager>())
        {
            manager.DestroyManager();
        }

        StartCoroutine(LastBreath());
    }

    private IEnumerator LastBreath()
    {
        float step = 0;

        do
        {
            step += .01f;
            material.SetFloat("_death", step);
            yield return new WaitForSecondsRealtime(.01f);
        } while(step < 1);

        OnDeath?.Invoke(this, new EnemyEventArgs(this));

        Destroy(gameObject);
    }

    public void UpdateHealth(float amount)
    {
        currentHealth += amount;
        if(currentHealth <= 0)
        {
            DestroyDamageable();
        }
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        float percentual = Mathf.Min(1 - currentHealth / maxHealth, 1);

        material.SetFloat("_damagePercentual", percentual);
    }
}
