using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VFXManager : MonoBehaviour, IManager
{
    [SerializeField] protected Material refMaterial;
    [SerializeField] protected ParticleSystem deathParticles;
    [SerializeField] [FMODUnity.EventRef] protected string deathSFX;
    protected AudioManager audioManager;

    protected Material instMaterial;


    protected virtual void Awake()
    {
        audioManager = AudioManager.Main;
        instMaterial = new Material(refMaterial);
        GetComponent<SpriteRenderer>().material = instMaterial;
    }

    public void DestroyManager()
    {
        StopAllCoroutines();
        Destroy(instMaterial);
        Destroy(this);
    }
}