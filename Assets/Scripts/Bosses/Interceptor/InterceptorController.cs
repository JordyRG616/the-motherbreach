using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterceptorController : BossController
{
    [SerializeField] private float offenseDuration;
    [Header("Second Phase Upgrades")]
    [SerializeField] private float emissionModifier;
    [Header("Third Phase Upgrade")]
    [SerializeField] private float intervalReduction;
    [Header("Interceptor SFX")]
    [SerializeField] [FMODUnity.EventRef] private string openWeaponsSFX;
    [SerializeField] [FMODUnity.EventRef] private string closeWeaponsSFX;

    protected override void SecondPhaseUpgrade()
    {
        GetComponent<ChargeAction>().activateWeaponry = true;
        GetComponent<DeployAction>().StartAction();

        var gunsToUpgrade = GetComponents<InterceptorArtillery>();

        foreach(ActionEffect gun in gunsToUpgrade)
        {
            var emission = gun.GetShooterSystem().emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(emission.rateOverTime.constant + emissionModifier);
        }
    }

    protected override void ThirdPhaseUpgrade()
    {
        GetComponent<DeployAction>().StartAction();
        intervalToCheck -= intervalReduction;
        waitTime = new WaitForSeconds(intervalToCheck);
    }

    public void PlayOpenSFX()
    {
        AudioManager.Main.RequestSFX(openWeaponsSFX);
    }

    public void PlayCloseSFX()
    {
        AudioManager.Main.RequestSFX(closeWeaponsSFX);
    }
}
