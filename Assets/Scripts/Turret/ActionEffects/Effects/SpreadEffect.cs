using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class SpreadEffect : ActionEffect
{
    [SerializeField] private float initialDuration;
    [SerializeField] private float initialSize;

    public override Stat specializedStat => Stat.Duration;

    public override Stat secondaryStat => Stat.Size;

    public override void SetData()
    {
        StatSet.Add(Stat.Size, initialSize);
        SetBulletSize();
        StatSet.Add(Stat.Duration, initialDuration);
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
        var main = shooterParticle.main;
        var lifetime = main.startLifetime;
        lifetime.constantMin = StatSet[Stat.Duration] - 1;
        lifetime.constantMax = StatSet[Stat.Duration] + 1;
        main.startLifetime = lifetime;
    }

    private void SetBulletSize()
    {
        var main = shooterParticle.main;
        Vector2 minMax = new Vector2();
        minMax.x = StatSet[Stat.Size] - 0.5f;
        minMax.y = StatSet[Stat.Size] + 0.5f;
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
        AudioManager.Main.RequestSFX(onShootSFX, out var instance);

        yield return new WaitForSeconds(duration + 1);

        AudioManager.Main.StopSFX(instance);
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage] * 0.05f);
        ApplyStatusEffect<Slug>(hitManager, 2f, new float[] {.66f});
    }

    private float MeanLifetime()
    {
        var min = shooterParticle.main.startLifetime.constantMin;
        var max = shooterParticle.main.startLifetime.constantMax;
        return (min + max) / 2;
    }

    public override string DescriptionText()
    {
        string description = "releases a cloud that lasts around " + StatColorHandler.StatPaint(MeanLifetime().ToString()) + " seconds, deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage on contact and applies " + KeywordHandler.KeywordPaint(Keyword.Slug) + " to the target";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        return StatColorHandler.StatPaint("next level:") + " duration + 0.5s";
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
        initialSize *= 1 + percentage;
    }
}
