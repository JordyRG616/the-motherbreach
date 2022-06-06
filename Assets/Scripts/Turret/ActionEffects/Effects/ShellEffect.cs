using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class ShellEffect : ActionEffect
{
    [SerializeField] private float initialBulletSize;
    [SerializeField] private float initialBurstSize;

    public override Stat specializedStat => Stat.Projectiles;

    public override Stat secondaryStat => Stat.Size;
    private string extraInfo = "";
    private bool exposeOn;

    public override void SetData()
    {
        StatSet.Add(Stat.Size, initialBulletSize);
        SetBulletSize();
        StatSet.Add(Stat.Projectiles, initialBurstSize);
        SetBurstSize();

        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetBulletSize();
        SetBurstSize();
    }

    private void SetBulletSize()
    {
        var main = shooterParticle.main;
        main.startSize = StatSet[Stat.Size];
    }

    private void SetBurstSize()
    {
        var module = shooterParticle.emission;
        var burst = module.GetBurst(0);
        burst.cycleCount = (int)StatSet[Stat.Projectiles];
        module.SetBurst(0, burst);
        // main.duration = ;
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
        if(exposeOn) ApplyStatusEffect<Expose>(hitManager, 4f, new float[] {.25f});
    }

    public override string DescriptionText()
    {
        string description = "shoots " + StatColorHandler.StatPaint(StatSet[Stat.Projectiles].ToString()) + " bursts of bullets. Each bullet deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage on hit";
        description += extraInfo;
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) 
            return StatColorHandler.StatPaint("next level:") + " +1 burst";

        else 
            return StatColorHandler.StatPaint("next level:") + " damage +15%";
    }

    public override void Shoot()
    {
        AudioManager.Main.RequestSFX(onShootSFX, out var sfxInstance);
        shooterParticle.Play();
    }

    public override void LevelUp(int toLevel)
    {
        exposeOn = true;

        extraInfo = " and apply 25% " + KeywordHandler.KeywordPaint(Keyword.Expose) + " for 4 seconds";
        keyword = Keyword.Expose;
    }

    public override void RemoveLevelUp()
    {
        exposeOn = false;
        
        extraInfo = "";
        keyword = Keyword.None;
    }

    public override void RaiseInitialSpecializedStat(float percentage)
    {
        initialBurstSize *= 1 + percentage;
    }

    public override void RaiseInitialSecondaryStat(float percentage)
    {
        initialBulletSize *= 1 + percentage;
    }
}
