using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamEffect : ActionEffect
{
    [SerializeField] private float duration;

    protected override void SetData()
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
        shooterParticle.Play();
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]/100);
        var burned = hitManager.GetComponent<ChemicalBurn>();
        if (burned == null) hitManager.gameObject.AddComponent<ChemicalBurn>();

    }

    public override string DescriptionText()
    {
        string description = "Releases a beam of energy for " + StatSet[Stat.Duration] + " seconds that deals " + StatSet[Stat.Damage] + " damage on contact and apply chemical burn.";
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
