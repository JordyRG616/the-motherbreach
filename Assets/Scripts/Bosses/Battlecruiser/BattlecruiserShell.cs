using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlecruiserShell : MonoBehaviour
{
    [SerializeField] private ParticleSystem rightShellDeployVFX;
    [SerializeField] private ParticleSystem leftShellDeployVFX;
    [SerializeField] [FMODUnity.EventRef] private string shellDeploySFX;
    [SerializeField] [FMODUnity.EventRef] private string burnerSFX;
    private FMOD.Studio.EventInstance burnerInstance;


    void Start()
    {
        AudioManager.Main.RequestSFX(burnerSFX, out burnerInstance);
    }

    public void PlayDeployEffect()
    {
        rightShellDeployVFX.Play();
        leftShellDeployVFX.Play();
        AudioManager.Main.RequestSFX(shellDeploySFX);
        AudioManager.Main.StopSFX(burnerInstance);
    }
}
