using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class EnhanceEffect : ActionEffect
{
    [SerializeField] [Range(0, 1)] private float percentage;
    private int triggers = 1;
    private Dictionary<ActionEffect, float> targetedWeaponsOriginalDamage = new Dictionary<ActionEffect, float>();

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
        for(int i = 0; i < triggers; i++)
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
            weapon.SetStat(Stat.Damage, damage * (1 + percentage));
        }
    }

    public override string DescriptionText()
    {
        return "raises the " + StatColorHandler.DamagePaint("damage") + " of a neighboring turret in " + StatColorHandler.StatPaint((percentage * 100).ToString()) + "% until the end of the wave";
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
