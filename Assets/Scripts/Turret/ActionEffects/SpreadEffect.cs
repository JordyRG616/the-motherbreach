using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadEffect : ActionEffect
{
    [SerializeField] private float durationModifier;
    [SerializeField] private float bulletSizeModifier;

    protected override void SetData()
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
        var main = shooter.main;
        Vector2 minMax = new Vector2();
        minMax.x = durationModifier + main.startLifetime.constantMin;
        minMax.y = durationModifier + main.startLifetime.constantMax;
        ParticleSystem.MinMaxCurve curve = new ParticleSystem.MinMaxCurve(minMax.x, minMax.y);
        main.startLifetime = curve;
    }

    private void SetBulletSize()
    {
        var main = shooter.main;
        Vector2 minMax = new Vector2();
        minMax.x = durationModifier + main.startSize.constantMin;
        minMax.y = durationModifier + main.startSize.constantMax;
        ParticleSystem.MinMaxCurve curve = new ParticleSystem.MinMaxCurve(minMax.x, minMax.y);
        main.startSize = curve;
    }

    public override void Shoot()
    {
        shooter.Play();
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]/100);
        Slug slugged;
        if (!hitManager.transform.TryGetComponent<Slug>(out slugged)) 
        {
            Debug.Log("working");
            hitManager.transform.gameObject.AddComponent<Slug>();
        }
    }

    public override string DescriptionText()
    {
        string description = "Releases a cloud for" + StatSet[Stat.Duration] + " seconds that deals " + StatSet[Stat.Damage] + " damage on contact and applies SLUG to the target";
        return description;
    }

    public override void LevelUp(int toLevel)
    {
        var duration = StatSet[Stat.Duration];
        duration *= (toLevel * 5) / 100;
        SetStat(Stat.Duration, duration);
    }
}
