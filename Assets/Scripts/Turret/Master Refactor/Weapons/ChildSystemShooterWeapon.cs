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
        if (initiated) return;
        StatSet.ForEach(x => x.Initiate(shooter, this));
        StatSet.ForEach(x => x.overwritten = true);
        dormentStats.ForEach(x => x.Initiate(shooter, this));
        StatSet = StatSet.OrderBy(x => x.sortingIndex).ToList();
        dormentStats = dormentStats.OrderBy(x => x.sortingIndex).ToList();

        var duration = TryGetComponent<Duration>(out var _d) ? GetStatValue<Duration>() : shooter.main.duration;

        waitForDuration = HasStat<Duration>() ? new WaitForSeconds(duration) : new WaitForSeconds(duration);
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

    private void OnDestroy()
    {
        if (!initiated) return;
        gameManager.OnGameStateChange -= HandleActivation;
    }

}
