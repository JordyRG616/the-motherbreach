using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitManager : MonoBehaviour, IManager
{    
    public IDamageable HealthInterface{get; private set;}
    private AudioManager audioManager;
    private float iFrameWindow;

    public void DestroyManager()
    {
        GetComponent<Collider2D>().enabled = false;
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
