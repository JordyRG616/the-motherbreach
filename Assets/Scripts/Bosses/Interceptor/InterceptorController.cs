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

    protected override void SecondPhaseUpgrade()
    {
        GetComponent<ChargeAction>().activateWeaponry = true;

        var gunsToUpgrade = GetComponents<InterceptorArtillery>();

        foreach(ActionEffect gun in gunsToUpgrade)
        {
            var emission = gun.GetShooterSystem().emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(emission.rateOverTime.constant + emissionModifier);
        }
    }

    protected override void ThirdPhaseUpgrade()
    {
        intervalToCheck -= intervalReduction;
        waitTime = new WaitForSeconds(intervalToCheck);
    }
}
