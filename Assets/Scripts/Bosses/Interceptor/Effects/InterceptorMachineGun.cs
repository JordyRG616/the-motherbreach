using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterceptorMachineGun : ActionEffect
{
    [SerializeField] private ParticleSystem trueWeapon;
    public override Stat specializedStat { get  ;  }
    public override Stat secondaryStat { get  ;  }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    public override string DescriptionText()
    {
        return "";
    }

    protected override void ManageSFX()
    {
        var amount = Mathf.Abs(cachedCount - trueWeapon.particleCount);

        if (trueWeapon.particleCount > cachedCount) 
        { 
            PlaySFX();
        } 

        cachedCount = trueWeapon.particleCount;
    }

    public override void LevelUp(int toLevel)
    {
        
    }
}
