using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class BeamEffect : ActionEffect
{
    [SerializeField] private float duration;

    public override void SetData()
    {
        StatSet.Add(Stat.Duration, duration);
        SetDuration();
        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetDuration();
    }

    private void SetDuration()
    {
        var main = shooterParticle.main;
        main.duration = StatSet[Stat.Duration];
    }

    public override void Shoot()
    {
        StartCoroutine(PlaySFX(StatSet[Stat.Duration]));
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
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]/100);
        var burned = hitManager.GetComponent<ChemicalBurn>();
        if (burned == null) hitManager.gameObject.AddComponent<ChemicalBurn>();

    }

    public override string DescriptionText()
    {
        string description = "releases a beam of energy for " + StatColorHandler.StatPaint(StatSet[Stat.Duration].ToString()) + " seconds that deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage on contact and apply " + KeywordHandler.KeywordPaint(Keyword.Burn);
        return description;
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 3 || toLevel == 5)
        {
            GainDuration();
        }
        else
        {
            GainDamage();
        }
    }

    private void GainDuration()
    {
        var _duration = StatSet[Stat.Duration];
        _duration *= 1.2f;
        SetStat(Stat.Duration, _duration);
    }

    private void GainDamage()
    {
        var damage = StatSet[Stat.Damage];
        damage *= 1.1f;
        SetStat(Stat.Damage, damage);
    }
}
