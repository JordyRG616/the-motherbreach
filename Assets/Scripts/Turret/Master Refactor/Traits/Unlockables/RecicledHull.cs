using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Traits/Unlockables/Recicled Hull", fileName = "Recicled Hull")]
public class RecicledHull : Trait
{
    [Range(0, 1)] public float percentage;
    private SellButton sellButton;
    private Weapon linkedWeapon;

    public override void ApplyEffect(Weapon weapon)
    {
        linkedWeapon = weapon;

        sellButton = FindObjectOfType<SellButton>(true);
        sellButton.OnTurretSell += RaiseIntegrity;

        weapon.GetComponentInParent<TurretManager>().OnTurretDestruction += Destroy;
    }

    private void RaiseIntegrity(object sender, System.EventArgs e)
    {
        if(linkedWeapon.HasStat<Health>(out var health))
        {
            health.ApplyPercentage(percentage);
        }
    }

    private void Destroy()
    {
        sellButton.OnTurretSell -= RaiseIntegrity;
    }

    public override string Description()
    {
        return "Raise this turret integrity by " + percentage * 100 + "% when you sell another turret";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (RecicledHull)ScriptableObject.CreateInstance(type);

        instance.percentage = percentage;

        return instance;
    }
}
