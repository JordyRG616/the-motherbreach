using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class LeechEffect : ActionEffect
{
    [SerializeField] private float projectiles;
    [SerializeField] [Range(0, 1)] private float leech;
    [SerializeField] private float initialArcSize;

    public override Stat specializedStat => Stat.Projectiles;

    public override Stat secondaryStat => Stat.Size;

    public override void SetData()
    {
        StatSet.Add(Stat.Projectiles, projectiles);
        SetProjectiles();
        StatSet.Add(Stat.Size, initialArcSize);
        SetSize();
        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetProjectiles();
    }

    private void SetProjectiles()
    {
        var emission = shooterParticle.emission;
        emission.rateOverTime = StatSet[Stat.Projectiles];
    }

    private void SetSize()
    {
        var main = shooterParticle.main;
        main.startSpeed = StatSet[Stat.Size];
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
        FindObjectOfType<ShipDamageController>().UpdateHealthNoEffects(StatSet[Stat.Damage] * leech);

    }

    public override string DescriptionText()
    {
        string description = "shoots " + StatColorHandler.StatPaint(StatSet[Stat.Projectiles]) + " returning bullets per second for 4 seconds that deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage]) + " damage and heals the ship for " + StatColorHandler.StatPaint(leech * 100) + "%  of the damage dealt";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " duration +20%";
        else return StatColorHandler.StatPaint("next level:") + " heals +15%";
        
    }

    public override void LevelUp(int toLevel)
    {
        leech += .3f;
        maxedOut = true;
    }

    public override void RemoveLevelUp()
    {
        if(!maxedOut) return;
        leech -= .3f;
        maxedOut = false;
    }

    public override void RaiseInitialSpecializedStat(float percentage)
    {
        projectiles *= 1 + percentage;
    }

    public override void RaiseInitialSecondaryStat(float percentage)
    {
        initialArcSize *= 1 + percentage;
    }
}
