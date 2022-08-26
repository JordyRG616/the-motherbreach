using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DelayedShooterWeapon : Weapon
{
    [SerializeField] private ParticleSystem delayedShooter;

    public override void Initiate()
    {
        if (initiated) return;
        StatSet.ForEach(x => x.Initiate(shooter, this));
        StatSet.ForEach(x => x.overwritten = true);
        dormentStats.ForEach(x => x.Initiate(shooter, this));
        StatSet = StatSet.OrderBy(x => x.sortingIndex).ToList();
        dormentStats = dormentStats.OrderBy(x => x.sortingIndex).ToList();

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
