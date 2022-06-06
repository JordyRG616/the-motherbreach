using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class PlasmaEffect : ActionEffect
{
    [SerializeField] private float duration;
    [SerializeField] private float initialEfficiency;
    private FMOD.Studio.EventInstance instance;

    public override Stat specializedStat => Stat.Duration;

    public override Stat secondaryStat => Stat.Efficiency;

    private bool maximized;

    public override void SetData()
    {
        StatSet.Add(Stat.Duration, duration);
        StatSet.Add(Stat.Efficiency, initialEfficiency);

        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
    }

    public override void Shoot()
    {
        StartCoroutine(PlaySFX(StatSet[Stat.Duration]));
        // AudioManager.Main.RequestSFX(onShootSFX, out sfxInstance);
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
        ApplyStatusEffect<Slug>(hitManager, StatSet[Stat.Duration], new float[] {StatSet[secondaryStat]});
    }

    public override string DescriptionText()
    {
        string description = "releases a stream of particles for " + shooterParticle.main.duration + " seconds that deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage to all enemies in the area and applies " + KeywordHandler.KeywordPaint(keyword) + " ("+ StatColorHandler.StatPaint(StatSet[Stat.Efficiency] * 100) + "% speed reduction for " + StatColorHandler.StatPaint(StatSet[Stat.Duration].ToString()) + " seconds)";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " slug duration + 1 s";
        else return StatColorHandler.StatPaint("next level:") + " duration + 25%";
        
    }

    public override void LevelUp(int toLevel)
    {
        var main = shooterParticle.main;
        main.duration *= 1.7f;
        maximized = true;
    }

    public override void RemoveLevelUp()
    {
        if(!maximized) return;
        var main = shooterParticle.main;
        main.duration /= 1.7f;
    }

    public override void RaiseInitialSpecializedStat(float percentage)
    {
        duration *= 1 + percentage;
    }

    public override void RaiseInitialSecondaryStat(float percentage)
    {
        initialEfficiency *= 1 + percentage;
    }
}
