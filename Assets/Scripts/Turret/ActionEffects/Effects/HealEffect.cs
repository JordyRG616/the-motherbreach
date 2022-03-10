using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class HealEffect : ActionEffect
{
    [SerializeField] [Range(0, 1)] private float initialHealPercentage;
    private SupportController controller;

    public override Stat specializedStat => Stat.Efficiency;

    public override Stat secondaryStat => Stat.Triggers;

    public override void SetData()
    {
        StatSet.Add(Stat.Efficiency, initialHealPercentage);
        StatSet.Add(Stat.Triggers, 1);
        base.SetData();

    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
    }

    public override void Initiate()
    {
        base.Initiate();

        controller = GetComponent<SupportController>();
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        
    }

    public override void Shoot()
    {
        for(int i = 0; i < StatSet[Stat.Triggers]; i++)
        {
            Invoke("Heal", .5f * i);
        }
    }

    private void Heal()
    {
        AudioManager.Main.RequestSFX(onShootSFX);
        var _target = controller.GetTarget();
        if(_target == null) return;
        target = _target.gameObject;
        shooterParticle.transform.position = target.transform.position;
        shooterParticle.transform.rotation = target.transform.rotation;
        shooterParticle.Play();
        var integrity = target.GetComponent<IntegrityManager>();
        integrity.UpdateHealth(integrity.GetMaxIntegrity() * StatSet[Stat.Efficiency]);
    }

    public override string DescriptionText()
    {
        return "heals a neighboring turret for " + StatColorHandler.DamagePaint((StatSet[Stat.Efficiency] * 100).ToString()) + "% of it's maximum health " + StatColorHandler.StatPaint(StatSet[Stat.Triggers].ToString()) + " times";
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
            var value = StatSet[Stat.Efficiency];
            value *= 1.15f;
            SetStat(Stat.Efficiency, value);
        }
        else
        {
            var _tr = StatSet[Stat.Triggers];
            _tr ++;
            SetStat(Stat.Triggers, _tr);
        }
    }

}
