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
        var main = shooter.main;
        main.duration = StatSet[Stat.Duration];
    }

    public override void Shoot()
    {
        shooter.Play();
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]/100);
        var burned = hitManager.GetComponent<ChemicalBurn>();
        if (burned == null) hitManager.gameObject.AddComponent<ChemicalBurn>();

    }
}
