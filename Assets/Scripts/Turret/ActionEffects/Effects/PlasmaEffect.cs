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

    public override void SetData()
    {
        StatSet.Add(Stat.Duration, duration);
        SetDuration();
        StatSet.Add(Stat.Efficiency, initialEfficiency);

        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetDuration();
    }

    private void SetDuration()
    {
        var main = shooterParticle.main;
        main.duration = StatSet[Stat.Duration];
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
        ApplyStatusEffect<Slug>(hitManager, StatSet[secondaryStat], new float[] {.66f});
    }

    public override string DescriptionText()
    {
        string description = "releases a stream of particles for " + StatColorHandler.StatPaint(StatSet[Stat.Duration].ToString()) + " seconds that deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage to all enemies in the area and applies " + KeywordHandler.KeywordPaint(keyword) + " for " + StatColorHandler.StatPaint(StatSet[secondaryStat].ToString()) + " seconds";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " slug duration + 1 s";
        else return StatColorHandler.StatPaint("next level:") + " duration + 25%";
        
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 3 || toLevel == 5)
        {
            GainEfficiency();
        }
        else
        {
            GainDuration();
        }
    }

    private void GainDuration()
    {
        var _duration = StatSet[Stat.Duration];
        _duration *= 1.25f;
        SetStat(Stat.Duration, _duration);
    }

    private void GainEfficiency()
    {
        var _eff = StatSet[secondaryStat];
        _eff += 1;
        SetStat(secondaryStat, _eff);
    }
}
