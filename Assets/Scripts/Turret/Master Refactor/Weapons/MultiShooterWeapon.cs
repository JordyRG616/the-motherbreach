using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MultiShooterWeapon : Weapon
{
    [SerializeField] private List<ParticleSystem> additionalShooters;

    public override void Initiate()
    {
        foreach(ParticleSystem shooter in additionalShooters)
        {
            StatSet.ForEach(x => x.ReceiveShooter(shooter));
            dormentStats.ForEach(x => x.ReceiveShooter(shooter));
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

    public override void LoadData(SaveFile saveFile)
    {
        var slotId = GetComponentInParent<TurretManager>().slotId;

        var list = new List<TurretStat>();
        list.AddRange(StatSet);
        list.AddRange(dormentStats);

        list.ForEach(x => x.Initiate(shooter, this));

        foreach (ParticleSystem shooter in additionalShooters)
        {
            list.ForEach(x => x.ReceiveShooter(shooter));
        }

        foreach (TurretStat stat in list)
        {
            var value = BitConverter.ToSingle(saveFile.GetValue(stat.publicName + slotId));
            stat.SetStatToValue(value);
        }
    }
}
