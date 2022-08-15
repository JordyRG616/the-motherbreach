using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Traits/Unlockables/Repair Kit", fileName = "Repair Kit")]
public class RepairKit : Trait
{
    [Range(0, 1)] public float percentage;
    private WaitForSeconds waitTime = new WaitForSeconds(1f);

    public override void ApplyEffect(Weapon weapon)
    {
        var integrityManager = weapon.GetComponentInParent<IntegrityManager>();

        weapon.StartCoroutine(Regenerate(integrityManager));
    }

    private IEnumerator Regenerate(IntegrityManager manager)
    {
        while(true)
        {
            if(!manager.IsAtMaxIntegrity())
            {
                var amount = manager.GetMaxIntegrity() * percentage;
                manager.UpdateHealth(amount);
            }

            yield return waitTime;
        }
    }

    public override string Description()
    {
        return "This turret regenerates " + percentage * 100 + "% of it's maximum health by second";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (RepairKit)ScriptableObject.CreateInstance(type);

        instance.percentage = percentage;

        return instance;
    }
}
