using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour, IManager
{    
    public IDamageable HealthInterface{get; private set;}

    public void DestroyManager()
    {
        GetComponent<Collider2D>().enabled = false;
        Destroy(this);
    }

    void Awake()
    {
        HealthInterface = GetComponent<IDamageable>();
    }

    void OnParticleCollision(GameObject other)
    {
        if(other.TryGetComponent<ActionEffect>(out ActionEffect action))
        {
            action.ApplyEffect(this);   
        }
    }
}
