using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAudioController : MonoBehaviour
{
    [SerializeField] [FMODUnity.EventRef] private string SFX;
    private ParticleSystem associatedEffect;
    private int cachedCount;
    private AudioManager audioManager;

    void Start()
    {
        associatedEffect = GetComponent<ParticleSystem>();
        audioManager = AudioManager.Main;
    }

    private void ManageSFX()
    {
        if(associatedEffect.isPlaying)
        {
            var amount = Mathf.Abs(cachedCount - associatedEffect.particleCount);

            if (associatedEffect.particleCount > cachedCount) 
            { 
                PlaySFX();
            } 

            cachedCount = associatedEffect.particleCount;
        }
    }

    private void PlaySFX()
    {
        audioManager.RequestSFX(SFX);
    }

    void LateUpdate()
    {
        ManageSFX();
    }
}
