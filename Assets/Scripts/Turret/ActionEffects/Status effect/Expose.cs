using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expose : StatusEffect
{
    private float ogMultiplier;
    private float percentage;
    private EnemyHealthController healthController;
    private BossHealthController bossHealth;

    public override Keyword Status => Keyword.Expose;

    protected override void ApplyEffect()
    {
        if(healthController)
        {
            ogMultiplier = healthController.damageMultiplier;
            healthController.damageMultiplier = ogMultiplier + percentage;
        }
        else if(bossHealth)
        {
            ogMultiplier = bossHealth.GetDamageReduction();
            bossHealth.SetDamageReduction(ogMultiplier - percentage);
        }
    }

    protected override void ExtraInitialize(params float[] parameters)
    {
        healthController = GetComponent<EnemyHealthController>();
        bossHealth = GetComponent<BossHealthController>();
        percentage = parameters[0];
    }

    protected override void RemoveEffect()
    {
        if(healthController)
        {
            healthController.damageMultiplier = ogMultiplier;
        }
        else if(bossHealth)
        {
            bossHealth.SetDamageReduction(ogMultiplier);
        }
    }
}
