using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncendiaryEffect : BaseEffectTemplate
{
    private ShipManager ship;

    void Awake()
    {
        ship = FindObjectOfType<ShipManager>();
        
        StartCoroutine(WaitForTrigger());
    }

    public override void ApplyEffect()
    {
        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            shooter.totalEffect += AddBurn;
        }
    }

    public void AddBurn(HitManager hitManager)
    {
        if(hitManager.TryGetComponent<ChemicalBurn>(out ChemicalBurn burn))
        {
            return;
        } else
        {
            hitManager.gameObject.AddComponent<ChemicalBurn>();
        }
        
    }

    private IEnumerator WaitForTrigger()
    {
        yield return new WaitUntil(() => BeamerCount() >= 4);

        ApplyEffect();
    }

    private int BeamerCount()
    {
        var weapons = ship.GetWeapons();
        int count = 0;

        foreach(ActionController weapon in weapons)
        {
            if(weapon.GetClasses().Contains(WeaponClass.Beamer))
            {
                count++;
            }
        }

        return count;
    }

    public override string DescriptionText()
    {
        string description = "";
        return description;
    }
}
