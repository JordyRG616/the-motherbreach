using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class MendingEffect : BaseEffectTemplate
{
    [SerializeField] [Range(0, 1)] private float healPercentage;
    [SerializeField] private float healFrequency;
    private float timer;

    public override void ApplyEffect()
    {
        var healthInterface = turretManager.GetComponent<IntegrityManager>();
        var health = healthInterface.GetCurrentIntegrity();
        health *= healPercentage;
        healthInterface.UpdateHealth(health);
        timer = 0;
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if(timer >= healFrequency)
        {
            ApplyEffect();
        }
    }
    public override string DescriptionText()
    {
        return "heals this turret for " + StatColorHandler.HealthPaint((healPercentage * 100).ToString()) + "% every " + StatColorHandler.StatPaint(healFrequency.ToString()) + " seconds";
    }
    

    public override string GetSpecialTrigger()
    {
        return "during wave:";
    }
}
