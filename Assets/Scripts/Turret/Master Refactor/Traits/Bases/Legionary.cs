using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Traits/Bases/Legionary", fileName = "Legionary")]
public class Legionary : Trait
{
    [Range(0, 1)] public float percentage;
    private ShipManager ship;
    private Weapon linkedWeapon;
    private TurretManager turretManager;
    private int count;
    private string key = "Legionary";

    public override void ApplyEffect(Weapon weapon)
    {
        ship = ShipManager.Main;
        linkedWeapon = weapon;

        ship.OnTraitCountUpdate += UpdateBuff;
        ship.ReceiveUpdatingTrait(key, this);

        turretManager = linkedWeapon.GetComponentInParent<TurretManager>();
        turretManager.OnTurretDestruction += RemoveTrait;
    }

    private void RemoveTrait()
    {
        ship.OnTraitCountUpdate -= UpdateBuff;
        turretManager.OnTurretDestruction -= RemoveTrait;

        ship.RemoveUpdatingTrait(key, this);
    }

    public void UpdateBuff(string key, int count)
    {
        Debug.Log(this.count + "/" + count);
        if (key != this.key) return;

        if (count == this.count) return;

        if(linkedWeapon.HasStat<Damage>(out var damage))
        {
            if(count > this.count) ApplyBuff(count - this.count, damage);
            if (count < this.count) RemoveBuff(this.count - count, damage);

            this.count = count;
        }
    }

    private void ApplyBuff(int difference, TurretStat stat)
    {
        for(int i = 0; i < difference; i++)
        {
            stat.ApplyPercentage(percentage);
        }
    }
    private void RemoveBuff(int difference, TurretStat stat)
    {
        for (int i = 0; i < difference; i++)
        {
            stat.RemovePercentage(percentage);
        }
    }

    public override string Description()
    {
        return "Raise the damage of this turret by " + percentage * 100 + "% for each legionary turret in the ship";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (Legionary)ScriptableObject.CreateInstance(type);

        instance.percentage = percentage;
        instance.count = 0;

        return instance;
    }
}
