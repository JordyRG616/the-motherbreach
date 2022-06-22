using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChildSystemShooterWeapon : Weapon
{
    [SerializeField] private ParticleSystem delayedShooter;
    [SerializeField] private ParticleSystem trueSystem;

    public override void Initiate()
    {
        StatSet.ForEach(x => x.Initiate(trueSystem, this));
        StatSet = StatSet.OrderBy(x => x.sortingIndex).ToList();

        waitForDuration = new WaitForSeconds(GetStatValue<Duration>());
        waitForCooldown = new WaitForSeconds(GetStatValue<Cooldown>());

        SetInitialEffect();

        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += HandleActivation;
        targetSystem = GetComponent<TargetSystem>();
        _animator = GetComponent<Animator>();
        restBar = GetComponentInParent<RestBarManager>();
    }

    protected override void SetInitialEffect()
    {
        totalEffect += ApplyDamage;
    }

    protected override void OpenFire()
    {
        base.OpenFire();
    }

    protected override void CeaseFire()
    {
        base.CeaseFire();
        delayedShooter.Stop();
    }

    public void A_DelayTrigger()
    {
        shooter.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        delayedShooter.Play();
    }
}
