using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class EnhanceEffect : ActionEffect
{
    [SerializeField] [Range(0, 1)] private float percentage;
    private Dictionary<ActionEffect, float> targetedWeaponsOriginalDamage = new Dictionary<ActionEffect, float>();

    public override Stat specializedStat => Stat.Efficiency;

    public override Stat secondaryStat => Stat.Triggers;

    public override void SetData()
    {
        StatSet.Add(Stat.Efficiency, percentage);
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

        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += ResetTurrets;
    }

    private void ResetTurrets(object sender, GameStateEventArgs e)
    {
        if(e.newState == GameState.OnReward)
        {
            foreach(ActionEffect weapon in targetedWeaponsOriginalDamage.Keys)
            {
                weapon.SetStat(Stat.Damage, targetedWeaponsOriginalDamage[weapon]);
            }
        }
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        
    }

    public override void Shoot()
    {
        for(int i = 0; i < StatSet[Stat.Triggers]; i++)
        {
            Invoke("Enhance", .5f * i);
        }
    }

    private void Enhance()
    {
        AudioManager.Main.RequestSFX(onShootSFX);
        target = GetComponentInParent<SupportController>().GetTarget().gameObject;
        shooterParticle.transform.position = target.transform.position;
        shooterParticle.transform.rotation = target.transform.rotation;
        shooterParticle.Play();
        var controller = target.GetComponentInChildren<ActionController>();

        foreach(ActionEffect weapon in controller.GetShooters())
        {
            if(!targetedWeaponsOriginalDamage.ContainsKey(weapon))
            {
                targetedWeaponsOriginalDamage.Add(weapon, weapon.StatSet[Stat.Damage]);
            }
            var damage = weapon.StatSet[Stat.Damage];
            weapon.SetStat(Stat.Damage, damage * (1 + StatSet[Stat.Efficiency]));
        }
    }

    public override string DescriptionText()
    {
        return "raises the " + StatColorHandler.DamagePaint("damage") + " of " + StatColorHandler.StatPaint(StatSet[Stat.Triggers].ToString()) + " neighboring turrets in " + StatColorHandler.StatPaint((StatSet[Stat.Efficiency] * 100).ToString()) + "% until the end of the wave";
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel < 5) return StatColorHandler.StatPaint("next level:") + " percentage + 3%";
        else return StatColorHandler.StatPaint("next level:") + " triggers one more time";
        
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel < 5)
        {
            var _eff = StatSet[Stat.Efficiency];
            _eff += 0.03f;
            SetStat(Stat.Efficiency, _eff);
        }
        else
        {
            var _tr = StatSet[Stat.Triggers];
            _tr ++;
            SetStat(Stat.Triggers, _tr);
        }
    }
}
