using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class EnhanceEffect : ActionEffect
{
    [SerializeField] [Range(0, 1)] private float percentage;
    private Dictionary<ActionEffect, float> targetedWeaponsOriginalDamage = new Dictionary<ActionEffect, float>();
    [SerializeField] private float maxRadius;
    private SupportController controller;
    private float ogRadius;

    private string extraInfo = "until the end of the wave";

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

        controller = GetComponent<SupportController>();

        ogRadius = GetComponent<CircleCollider2D>().radius;
    }

    private void ResetTurrets(object sender, GameStateEventArgs e)
    {
        if (maxedOut) return;
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
        var _t = controller.GetTarget();
        if (_t == null) return;
        target = _t.gameObject;
        shooterParticle.transform.position = target.transform.position;
        shooterParticle.transform.rotation = target.transform.rotation;
        shooterParticle.Play();
        var _c = target.GetComponentInChildren<ActionController>();

        foreach(ActionEffect weapon in _c.GetShooters())
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
        return "raises the " + StatColorHandler.DamagePaint("damage") + " of a neighbouring turret in " + StatColorHandler.StatPaint((StatSet[Stat.Efficiency] * 100).ToString()) + "% " + extraInfo + ". repeats " + StatColorHandler.StatPaint(StatSet[Stat.Triggers]) + " times";
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel < 5) return StatColorHandler.StatPaint("next level:") + " percentage + 3%";
        else return StatColorHandler.StatPaint("next level:") + " triggers one more time";
        
    }

    public override void LevelUp(int toLevel)
    {
        extraInfo = "permanently";
        maxedOut = true;
    }

    public override void RemoveLevelUp()
    {
        if (!maxedOut) return;
        extraInfo = "until the end of the wave";
        maxedOut = false;
    }

    public override void RaiseInitialSpecializedStat(float percentage)
    {
        this.percentage *= 1 + percentage;
    }

    public override void RaiseInitialSecondaryStat(float percentage)
    {
        
    }
}
