using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Traits/Unlockables/Linked Augmenter", fileName = "Linked Augmenter")]
public class LinkedAugmenter : Trait
{
    [Range(0, 1)] public float percentage;
    private RewardManager rewardManager;
    private Weapon linkedWeapon;

    public override void ApplyEffect(Weapon weapon)
    {
        linkedWeapon = weapon;

        rewardManager = RewardManager.Main;
        rewardManager.OnTurretBuild += RaiseDamage;

        weapon.GetComponentInParent<TurretManager>().OnTurretDestruction += Destroy;
    }

    private void Destroy()
    {
        rewardManager.OnTurretBuild -= RaiseDamage;
    }

    private void RaiseDamage(object sender, BuildEventArgs e)
    {
        if(linkedWeapon.HasStat<Damage>(out var damage))
        {
            damage.ApplyPercentage(percentage);
        }
    }

    public override string Description()
    {
        return "Raise this turret damage by " + percentage * 100 + "% when you build a turret (including this one)";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (LinkedAugmenter)ScriptableObject.CreateInstance(type);

        instance.percentage = percentage;

        return instance;
    }
}
