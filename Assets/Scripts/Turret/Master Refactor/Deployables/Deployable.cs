using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Deployable : MonoBehaviour
{
    [SerializeField] protected float lifetime;
    protected float remainingLifetime;
    [HideInInspector] public bool deployed;
    private WaveManager waveManager;

    public delegate void RedockEvent(Deployable deployable);
    public RedockEvent OnRedock;

    protected virtual void OnEnable()
    {
        remainingLifetime = lifetime;
        waveManager = WaveManager.Main;
        waveManager.OnWaveEnd += GameManager_OnGameStateChange;
    }

    private void GameManager_OnGameStateChange(object sender, EndWaveEventArgs e)
    {
        DisableDeployable();
        waveManager.OnWaveEnd -= GameManager_OnGameStateChange;
    }

    public abstract void Initialize();
    public abstract void Launch();

    protected virtual void Update()
    {
        if (deployed)
        {
            remainingLifetime -= Time.deltaTime;
            if (remainingLifetime <= 0) DisableDeployable();
        }
    }

    protected virtual void DisableDeployable()
    {
        OnRedock?.Invoke(this);
        gameObject.SetActive(false);
        waveManager.OnWaveEnd -= GameManager_OnGameStateChange;
    }

    private void OnDestroy()
    {
        waveManager.OnWaveEnd -= GameManager_OnGameStateChange;
    }

    public virtual void SetLifetime(float value)
    {
        lifetime = value;
    }
}
