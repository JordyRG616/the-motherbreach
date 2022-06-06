using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class StingerEffect : ActionEffect
{
   [SerializeField] private float initialProjectiles;
    [SerializeField] private float initialDuration;
    [SerializeField] private ParticleSystem subShooter;

    public override Stat specializedStat => Stat.Duration;

    public override Stat secondaryStat => Stat.Projectiles;

    public override void SetData()
    {
        StatSet.Add(Stat.Duration, initialDuration);
        SetDuration();
        StatSet.Add(Stat.Projectiles, initialProjectiles);
        SetProjectiles();

        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetProjectiles();
        SetDuration();
    }

    private void SetDuration()
    {
        var main = subShooter.main;
        var lifetime = main.startLifetime;
        lifetime.constantMin = StatSet[Stat.Duration] - 1;
        lifetime.constantMax = StatSet[Stat.Duration] + 1;
        main.startLifetime = lifetime;
    }

    private void SetProjectiles()
    {
        var emission = shooterParticle.emission;
        var burst = emission.GetBurst(0);
        burst.count = new ParticleSystem.MinMaxCurve(StatSet[Stat.Projectiles]);
        emission.SetBurst(0, burst);
    }

    public override void Shoot()
    {
        StartCoroutine(PlaySFX(shooterParticle.main.duration));
        shooterParticle.Play();
    }

    private IEnumerator PlaySFX(float duration)
    {
        AudioManager.Main.RequestSFX(onShootSFX, out var instance);

        yield return new WaitForSeconds(duration + 1);

        AudioManager.Main.StopSFX(instance);
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    private float MeanLifetime()
    {
        var min = subShooter.main.startLifetime.constantMin;
        var max = subShooter.main.startLifetime.constantMax;
        return (min + max) / 2;
    }

    public override string DescriptionText()
    {
        string description = "shoots " + StatColorHandler.StatPaint(StatSet[Stat.Projectiles]) + " bolts that explodes in a cloud that deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage]) + " damage on contact";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel < 4) return StatColorHandler.StatPaint("next level:") + " duration + 0.5";
        else return StatColorHandler.StatPaint("next level:") + " cloud radius + 1";
    }

    public override void LevelUp(int toLevel)
    {
        
    }

    public override void RaiseInitialSpecializedStat(float percentage)
    {
        initialDuration *= 1 + percentage;
    }

    public override void RaiseInitialSecondaryStat(float percentage)
    {
        initialProjectiles *= 1 + percentage;
    }
}
