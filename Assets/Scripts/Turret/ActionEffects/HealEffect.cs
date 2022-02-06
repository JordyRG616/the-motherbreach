using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class HealEffect : ActionEffect
{
    private int triggers = 1;

    public override void ApplyEffect(HitManager hitManager)
    {
        
    }

    public override void Shoot()
    {
        for(int i = 0; i < triggers; i++)
        {
            Invoke("Heal", .5f * i);
        }
    }

    private void Heal()
    {
        AudioManager.Main.RequestSFX(onShootSFX);
        target = GetComponentInParent<SupportController>().GetTarget().gameObject;
        shooterParticle.transform.position = target.transform.position;
        shooterParticle.transform.rotation = target.transform.rotation;
        shooterParticle.Play();
        target.GetComponent<IntegrityManager>().UpdateHealth(StatSet[Stat.Damage]);
    }

    public override string DescriptionText()
    {
        return "heals a neighboring turret for " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " points of damage " + StatColorHandler.StatPaint(triggers.ToString()) + " times";
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel < 5) return StatColorHandler.StatPaint("next level:") + " heals + 15%";
        else return StatColorHandler.StatPaint("next level:") + " triggers one more time";
        
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel < 5)
        {
            var value = StatSet[Stat.Damage];
            value *= 1.15f;
            SetStat(Stat.Damage, value);
        }
        else
        {
            triggers ++;
        }
    }

}
