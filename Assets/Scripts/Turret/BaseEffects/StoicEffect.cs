using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoicEffect : BaseEffectTemplate
{
    [SerializeField] private float percentage;
    private ShipManager ship;

    void Awake()
    {
        ship = FindObjectOfType<ShipManager>();
    }

    public override void ApplyEffect()
    {
        var integrityManager = GetComponentInParent<IntegrityManager>();
        float totalPercentage = BomberCount() * percentage;
        integrityManager.RaiseMaxIntegrityByPercentage(totalPercentage);
    }

    private int BomberCount()
    {
        var weapons = ship.GetWeapons();
        int count = 0;

        foreach(ActionController weapon in weapons)
        {
            if(weapon.GetClasses().Contains(WeaponClass.Bomber))
            {
                count++;
            }
        }

        return count;
    }
}
