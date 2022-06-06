using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailshipBeam : ActionEffect
{
    public override Stat specializedStat => Stat.Damage;
    public override Stat secondaryStat => Stat.Rest;

    [SerializeField] private ParticleSystem init;
    [SerializeField] private ParticleSystem afterEffect;
    [SerializeField] private float initDuration;
    [SerializeField] private float beamDuration;
    [SerializeField] private JailshipWeaponryController controller;
    private WaitForSeconds initTime;
    private WaitForSeconds beamTime;


    public override void Initiate()
    {
        base.Initiate();

        initTime = new WaitForSeconds(initDuration);
        beamTime = new WaitForSeconds(beamDuration);
    }

    public override void Shoot()
    {
        if(!shooterParticle.isEmitting)
        {
            PlaySFX();
            StartCoroutine(HandleActivation());
        }
    }

    private IEnumerator HandleActivation()
    {
        controller.fixedRotation = true;

        init.Play();

        yield return initTime;

        init.Stop();
        shooterParticle.Play();

        yield return beamTime;

        shooterParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        afterEffect.Play();

        controller.fixedRotation = false;
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    public override string DescriptionText()
    {
        return "";
    }

    public override void LevelUp(int toLevel)
    {

    }
}
