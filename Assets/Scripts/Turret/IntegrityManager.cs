using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegrityManager : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxIntegrity;
    [SerializeField] private SpriteRenderer barRenderer;
    private float currentIntegrity;

    void Awake()
    {
        currentIntegrity = maxIntegrity;
    }

    public void DestroyDamageable()
    {
        foreach(IManager manager in GetComponents<IManager>())
        {
            manager.DestroyManager();
        }

        Destroy(gameObject);
    }

    public void UpdateHealth(float amount)
    {
        currentIntegrity += amount;
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

}
