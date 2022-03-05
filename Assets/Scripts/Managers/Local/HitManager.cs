using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HitManager : MonoBehaviour, IManager
{    
    public IDamageable HealthInterface{get; private set;}
    private AudioManager audioManager;
    private float iFrameWindow;
    private List<StatusEffect> effects = new List<StatusEffect>();
    public GameObject lastAttacker {get; private set;}

    public event EventHandler<HitEventArgs> OnHit;
    public event EventHandler<HitEventArgs> OnDeath;

    public void DestroyManager()
    {
        GetComponent<Collider2D>().enabled = false;
        OnDeath?.Invoke(this, new HitEventArgs(lastAttacker));
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
                action.PassTarget(this, out var damage);
                lastAttacker = action.associatedEffect.gameObject;
                OnHit?.Invoke(this, new HitEventArgs(damage, lastAttacker));
                iFrameWindow = 0;
            }
        }
    }

    public void ReceiveTriggerEffect(EffectMediator mediator)
    {
        if(iFrameWindow >= 0.05f)
        {
            mediator.PassTarget(this, out var damage);
            lastAttacker = mediator.associatedEffect.gameObject;
            OnHit?.Invoke(this, new HitEventArgs(damage, lastAttacker));
            iFrameWindow = 0;
        }
    }

    void FixedUpdate()
    {
        iFrameWindow += Time.fixedDeltaTime;
    }

    public void ReceiveEffect(StatusEffect effect)
    {
        effects.Add(effect);
    }

    public void RemoveEffect(StatusEffect effect)
    {
        effects.Remove(effect);
    }

    public bool IsUnderEffect<T>() where T : StatusEffect
    {
        var status = GetComponent<T>();
        if(status == null) return false;
        return true;
    }
}
