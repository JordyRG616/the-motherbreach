using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jailship_Shotgun : ActionEffect
{
    public override Stat specializedStat => Stat.Damage;
    public override Stat secondaryStat => Stat.Rest;


    public override void Shoot()
    {
        if(!shooterParticle.isEmitting)
        {
            PlaySFX();
            shooterParticle.Play(true);
        }
    }

    // private IEnumerator HandleActivation()
    // {
    //     controller.fixedRotation = true;

    //     init.Play();

    //     yield return initTime;

    //     init.Stop();
    //     shooterParticle.Play();

    //     yield return beamTime;

    //     shooterParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    //     afterEffect.Play();

    //     controller.fixedRotation = false;
    // }

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
