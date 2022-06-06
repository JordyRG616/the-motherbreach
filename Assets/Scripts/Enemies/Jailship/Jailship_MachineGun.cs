using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jailship_MachineGun : ActionEffect
{
    public override Stat specializedStat => Stat.Damage;
    public override Stat secondaryStat => Stat.Rest;

    [SerializeField] private ParticleSystem init;
    [SerializeField] private float initDuration;
    [SerializeField] private JailshipWeaponryController controller;
    private WaitForSeconds initTime;


    public override void Initiate()
    {
        base.Initiate();

        initTime = new WaitForSeconds(initDuration);
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

        init.Play();

        yield return initTime;
        controller.fixedRotation = true;

        init.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        shooterParticle.Play();

        yield return new WaitForSeconds(shooterParticle.main.duration);

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
