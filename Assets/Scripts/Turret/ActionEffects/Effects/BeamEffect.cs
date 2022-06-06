using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class BeamEffect : ActionEffect
{
    [SerializeField] private float duration;
    [SerializeField] private float initialBulletSize;
    private FMOD.Studio.EventInstance instance;
    private string acidInfo = "";

    public override Stat specializedStat => Stat.Duration;

    public override Stat secondaryStat => Stat.Size;

    public override void SetData()
    {
        StatSet.Add(Stat.Duration, duration);
        SetDuration();
        StatSet.Add(Stat.Size, initialBulletSize);
        SetBulletSize();
        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetDuration();
        SetBulletSize();
    }

    private void SetDuration()
    {
        var main = shooterParticle.main;
        main.duration = StatSet[Stat.Duration];
    }

    private void SetBulletSize()
    {
        var main = shooterParticle.main;
        main.startSize = StatSet[Stat.Size];
    }

    public override void Shoot()
    {
        StartCoroutine(PlaySFX(StatSet[Stat.Duration]));
        // AudioManager.Main.RequestSFX(onShootSFX, out var sfxInstance);
        shooterParticle.Play();
    }

    private IEnumerator PlaySFX(float duration)
    {
        AudioManager.Main.RequestSFX(onShootSFX, out var instance);

        yield return new WaitForSeconds(duration + 1);

        AudioManager.Main.StopSFX(instance);
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    public override string DescriptionText()
    {
        string description = "releases a beam of energy for " + StatColorHandler.StatPaint(StatSet[Stat.Duration].ToString()) + " seconds that deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage on contact";
        description += acidInfo;
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " duration + 20%";
        else return StatColorHandler.StatPaint("next level:") + " damage + 10%";
        
    }

    public override void LevelUp(int toLevel)
    {
        totalEffect += ApplyAcid;
        acidInfo = " and applies " + KeywordHandler.KeywordPaint(Keyword.Acid) + " (deals " + StatColorHandler.DamagePaint(20) + " damage over 2 seconds)";
    }

    public override void RemoveLevelUp()
    {
        Debug.Log("removed");
        totalEffect -= ApplyAcid;
        acidInfo = "";
    }

    private void ApplyAcid(HitManager hitManager)
    {
        ApplyStatusEffect<Acid>(hitManager, 2f, new float[] {1f, .1f});
    }

    public override void RaiseInitialSpecializedStat(float percentage)
    {
        duration *= 1 + percentage;
    }

    public override void RaiseInitialSecondaryStat(float percentage)
    {
        initialBulletSize *= percentage;
    }
}
