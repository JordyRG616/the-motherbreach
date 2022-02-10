using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class StingerEffect : ActionEffect
{
   [SerializeField] private float durationModifier;
    [SerializeField] private float bulletSizeModifier;
    [SerializeField] private ParticleSystem subShooter;

    public override void SetData()
    {
        StatSet.Add(Stat.BulletSize, bulletSizeModifier);
        SetBulletSize();
        StatSet.Add(Stat.Duration, durationModifier);
        SetDuration();

        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetDuration();
        SetBulletSize();
    }

    private void SetDuration()
    {
        var main = subShooter.main;
        var lifetime = main.startLifetime;
        // Vector2 minMax = new Vector2();
        lifetime.constantMin = StatSet[Stat.Duration] + main.startLifetime.constantMin;
        lifetime.constantMax = StatSet[Stat.Duration] + main.startLifetime.constantMax;
        main.startLifetime = lifetime;
        // ParticleSystem.MinMaxCurve curve = new ParticleSystem.MinMaxCurve(minMax.x, minMax.y);
    }

    private void SetBulletSize()
    {
        var main = subShooter.main;
        Vector2 minMax = new Vector2();
        minMax.x = durationModifier + main.startSize.constantMin;
        minMax.y = durationModifier + main.startSize.constantMax;
        ParticleSystem.MinMaxCurve curve = new ParticleSystem.MinMaxCurve(minMax.x, minMax.y);
        main.startSize = curve;
    }

    public override void Shoot()
    {
        StartCoroutine(PlaySFX(shooterParticle.main.duration));
        shooterParticle.Play();
    }

    private IEnumerator PlaySFX(float duration)
    {
        Debug.Log(duration);
        AudioManager.Main.RequestSFX(onShootSFX, out var instance);

        yield return new WaitForSeconds(duration + 1);

        AudioManager.Main.StopSFX(instance);
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
        // Slug slugged;

        // if(hitManager.TryGetComponent<BossController>(out var boss))
        // {
        //     if(!hitManager.gameObject.TryGetComponent<Slug>(out slugged)) 
        //     {
        //         hitManager.gameObject.AddComponent<Slug>();
        //     }
        //     return;
        // }
        // else if(!hitManager.transform.parent.TryGetComponent<Slug>(out slugged)) 
        // {
        //     hitManager.transform.parent.gameObject.AddComponent<Slug>();
        // }
    }

    private float MeanLifetime()
    {
        var min = subShooter.main.startLifetime.constantMin;
        var max = subShooter.main.startLifetime.constantMax;
        return (min + max) / 2;
    }

    public override string DescriptionText()
    {
        string description = "releases a burst of bullets that explodes in a cloud that lasts for " + StatColorHandler.StatPaint(MeanLifetime().ToString()) + " seconds and deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage on contact to the target";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel < 4) return StatColorHandler.StatPaint("next level:") + " duration + 0.5";
        else return StatColorHandler.StatPaint("next level:") + " cloud radius +" + nextLevel / 10f;
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel < 4) GainDuration();
        else GainSize(toLevel / 10);
    }

    private void GainSize(float percentage)
    {
        
        var size = StatSet[Stat.BulletSize];
        size += percentage;
        SetStat(Stat.BulletSize, size);
    }

    private void GainDuration()
    {
        var duration = StatSet[Stat.Duration];
        duration += 0.5f;
        SetStat(Stat.Duration, duration);
    }
}
