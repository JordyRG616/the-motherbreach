using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegrityManager : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxIntegrity;
    private float currentIntegrity;

    void Awake()
    {
        currentIntegrity = maxIntegrity;
    }

    public void DestroyDamageable()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateHealth(float amount)
    {
        currentIntegrity += amount;
    }

    public void UpdateHealthBar()
    {
        throw new System.NotImplementedException();
    }

}
