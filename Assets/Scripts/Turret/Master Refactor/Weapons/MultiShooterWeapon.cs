using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShooterWeapon : Weapon
{
    [SerializeField] private List<ParticleSystem> additionalShooters;

    public override void Initiate()
    {
        foreach(ParticleSystem shooter in additionalShooters)
        {
            StatSet.ForEach(x => x.ReceiveShooter(shooter));
        }

        base.Initiate();
    }

    protected override void SetInitialEffect()
    {
        totalEffect += ApplyDamage;
    }

    protected override void OpenFire()
    {
        base.OpenFire();
        additionalShooters.ForEach(x => x.Play());
    }

    protected override void CeaseFire()
    {
        base.CeaseFire();
        additionalShooters.ForEach(x => x.Stop(true));
    }
}
