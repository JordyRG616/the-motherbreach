using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Traits/Bases/Symbiotic", fileName = "Symbiotic")]
public class Symbiotic : Trait
{
    [Range(0, 1)] public float percentage;
    private ShipManager ship;
    private Weapon linkedWeapon;
    private TurretManager turretManager;
    private int count;
    private string key = "Symbiotic";
    private WaitForSeconds waitTime = new WaitForSeconds(1f);

    public override void ApplyEffect(Weapon weapon)
    {
        ship = ShipManager.Main;
        linkedWeapon = weapon;

        ship.OnTraitCountUpdate += UpdateBuff;
        ship.ReceiveUpdatingTrait(key, this);

        turretManager = linkedWeapon.GetComponentInParent<TurretManager>();
        turretManager.OnTurretDestruction += RemoveTrait;

        var integrityManager = weapon.GetComponentInParent<IntegrityManager>();

        weapon.StartCoroutine(Regenerate(integrityManager));
    }

    private void RemoveTrait()
    {
        ship.OnTraitCountUpdate -= UpdateBuff;
        turretManager.OnTurretDestruction -= RemoveTrait;

        ship.RemoveUpdatingTrait(key, this);
    }

    public void UpdateBuff(string key, int count)
    {
        if (key != this.key) return;
        this.count = count;

        //if (count == this.count) return;

        //if (linkedWeapon.HasStat<Cooldown>(out var cooldown))
        //{
        //    if (count > this.count) ApplyBuff(count - this.count, cooldown);
        //    if (count < this.count) RemoveBuff(this.count - count, cooldown);

        //    this.count = count;
        //}
    }

    private IEnumerator Regenerate(IntegrityManager manager)
    {
        while (true)
        {
            if (!manager.IsAtMaxIntegrity())
            {
                var amount = manager.GetMaxIntegrity() * percentage * count;
                manager.UpdateHealth(amount);
            }

            yield return waitTime;
        }
    }

    //private void ApplyBuff(int difference, TurretStat stat)
    //{
    //    for (int i = 0; i < difference; i++)
    //    {
    //        stat.ApplyPercentage(-percentage);
    //    }
    //}
    //private void RemoveBuff(int difference, TurretStat stat)
    //{
    //    for (int i = 0; i < difference; i++)
    //    {
    //        stat.RemovePercentage(-percentage);
    //    }
    //}

    public override string Description()
    {
        return "This turret receive " + percentage * 100 + " % of regeneration for each symbiotic turret in the ship";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (Symbiotic)ScriptableObject.CreateInstance(type);

        instance.percentage = percentage;
        instance.count = 0;

        return instance;
    }
}
