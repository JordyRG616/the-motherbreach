using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

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
        var main = shooterParticle.main;
        Vector2 minMax = new Vector2();
        minMax.x = durationModifier + main.startLifetime.constantMin;
        minMax.y = durationModifier + main.startLifetime.constantMax;
        ParticleSystem.MinMaxCurve curve = new ParticleSystem.MinMaxCurve(minMax.x, minMax.y);
        main.startLifetime = curve;
    }

    private void SetBulletSize()
    {
        var main = shooterParticle.main;
        Vector2 minMax = new Vector2();
        minMax.x = durationModifier + main.startSize.constantMin;
        minMax.y = durationModifier + main.startSize.constantMax;
        ParticleSystem.MinMaxCurve curve = new ParticleSystem.MinMaxCurve(minMax.x, minMax.y);
        main.startSize = curve;
    }

    public override void Shoot()
    {
        shooterParticle.Play();
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]/100);
        Slug slugged;
        if (!hitManager.transform.parent.TryGetComponent<Slug>(out slugged)) 
        {
            Debug.Log("working");
            hitManager.transform.parent.gameObject.AddComponent<Slug>();
        }
    }

    public override string DescriptionText()
    {
        string description = "releases a cloud for" + StatColorHandler.StatPaint(StatSet[Stat.Duration].ToString()) + " seconds that deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage on contact and applies " + KeywordHandler.KeywordPaint(Keyword.Slug) + " to the target";
        return description;
    }

    public override void LevelUp(int toLevel)
    {
        var duration = StatSet[Stat.Duration];
        duration *= (toLevel * 5) / 100;
        SetStat(Stat.Duration, duration);
    }
}
