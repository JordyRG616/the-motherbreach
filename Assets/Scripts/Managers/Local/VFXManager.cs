using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VFXManager : MonoBehaviour, IManager
{
    [SerializeField] protected Material refMaterial;
    [SerializeField] protected ParticleSystem deathParticles;

    protected Material instMaterial;


    protected virtual void Awake()
    {
        instMaterial = new Material(refMaterial);
        GetComponent<SpriteRenderer>().material = instMaterial;
    }

    public void DestroyManager()
    {
        StopAllCoroutines();
        Destroy(instMaterial);
    }
}
