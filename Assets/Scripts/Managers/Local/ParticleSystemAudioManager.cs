using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemAudioManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem associatedPS;
    [SerializeField] [FMODUnity.EventRef] private string eventPath;
    private AudioManager audioManager;
    private int currentParticleCount;

    void Start()
    {
        audioManager = AudioManager.Main;
    }

    void Update()
    {
        if(currentParticleCount < associatedPS.particleCount)
        {
            for(int i = 0; i <= associatedPS.particleCount - currentParticleCount; i++)
            {
                Invoke("PlaySFX", i/10);
            }
        }

        currentParticleCount = associatedPS.particleCount;
    }

    private void PlaySFX()
    {
        audioManager.RequestSFX(eventPath);
    }
}
