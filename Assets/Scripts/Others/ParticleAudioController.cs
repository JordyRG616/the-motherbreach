using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAudioController : MonoBehaviour
{
    [SerializeField] [FMODUnity.EventRef] private string SFX;
    [SerializeField] private bool singleSFX;
    private ParticleSystem associatedEffect;
    private int cachedCount;
    private AudioManager audioManager;
    private FMOD.Studio.EventInstance instance;

    void Start()
    {
        associatedEffect = GetComponent<ParticleSystem>();
        audioManager = AudioManager.Main;
        if (singleSFX) StartCoroutine(ManageSingleSFX());
    }

    private void ManageSFX()
    {
        if(associatedEffect.isPlaying)
        {
            if (associatedEffect.particleCount > cachedCount) 
            { 
                PlaySFX();
            } 

            cachedCount = associatedEffect.particleCount;
        }
    }

    private IEnumerator ManageSingleSFX()
    {
        while(true)
        {
            yield return new WaitUntil(() => associatedEffect.isPlaying);

            audioManager.RequestSFX(SFX, out instance);

            yield return new WaitUntil(() => associatedEffect.isStopped);

            audioManager.StopSFX(instance);
        }
    }

    private void PlaySFX()
    {
        audioManager.RequestSFX(SFX);
    }

    void LateUpdate()
    {
        if (singleSFX) return;
        ManageSFX();
    }
}
