using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HitManager : MonoBehaviour, IManager
{    
    public IDamageable HealthInterface{get; private set;}
    private AudioManager audioManager;
    private float iFrameWindow;

    public event EventHandler OnHit;
    public event EventHandler OnDeath;

    public void DestroyManager()
    {
        GetComponent<Collider2D>().enabled = false;
        OnDeath?.Invoke(this, EventArgs.Empty);
        Destroy(this);
    }

    void Awake()
    {
        audioManager = AudioManager.Main;

        HealthInterface = GetComponent<IDamageable>();
    }

    void OnParticleCollision(GameObject other)
    {
        if(other.TryGetComponent<EffectMediator>(out EffectMediator action))
        {
            if(iFrameWindow >= 0.05f)
            {
                action.PassTarget(this);
                OnHit?.Invoke(this, EventArgs.Empty);
                iFrameWindow = 0;
            }
        }
    }

    public void ReceiveTriggetEffect(EffectMediator mediator)
    {
        if(iFrameWindow >= 0.05f)
        {
            mediator.PassTarget(this);
            iFrameWindow = 0;
        }
    }

    void FixedUpdate()
    {
        iFrameWindow += Time.fixedDeltaTime;
    }
}
