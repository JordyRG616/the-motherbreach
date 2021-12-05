using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SluggingEffect : BaseEffectTemplate
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
            shooter.totalEffect += AddSlug;
        }
    }

    public void AddSlug(HitManager hitManager)
    {
        if(hitManager.TryGetComponent<Slug>(out Slug slug))
        {
            return;
        } else
        {
            hitManager.gameObject.AddComponent<Slug>();
        }
    }

    private IEnumerator WaitForTrigger()
    {
        yield return new WaitUntil(() => SpreaderCount() >= 4);

        ApplyEffect();
    }

    private int SpreaderCount()
    {
        var weapons = ship.GetWeapons();
        int count = 0;

        foreach(ActionController weapon in weapons)
        {
            if(weapon.GetClasses().Contains(WeaponClass.Spreader))
            {
                count++;
            }
        }

        return count;
    }
}
