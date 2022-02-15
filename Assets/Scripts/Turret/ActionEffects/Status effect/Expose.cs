using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expose : StatusEffect
{
    private float ogMultiplier;
    private float percentage;
    private EnemyHealthController healthController;

    public override Keyword Status => Keyword.Expose;

    protected override void ApplyEffect()
    {
        ogMultiplier = healthController.damageMultiplier;
        healthController.damageMultiplier = ogMultiplier - percentage;
    }

    protected override void ExtraInitialize(params float[] parameters)
    {
        healthController = target.GetComponent<EnemyHealthController>();
        percentage = parameters[0];
    }

    protected override void RemoveEffect()
    {
        healthController.damageMultiplier = ogMultiplier;
    }
}
