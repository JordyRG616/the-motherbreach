using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class NukeEffect : ActionEffect
{
    [SerializeField] private float blastDuration;
    [SerializeField] private float initalRate;
    [SerializeField] private ParticleSystem subEmitter;
    [SerializeField] [FMODUnity.EventRef] private string explosionSFX;
    private FMOD.Studio.EventInstance instance;

    public override Stat specializedStat => Stat.Rate;

    public override Stat secondaryStat => Stat.Duration;

    public override void SetData()
    {
        StatSet.Add(Stat.Duration, blastDuration);
        SetDuration();
        StatSet.Add(Stat.Rate, initalRate);
        SetRate();
        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetRate();
        SetDuration();
    }

    private void SetDuration()
    {
        var main = subEmitter.main;
        main.startLifetime = StatSet[Stat.Duration];
    }

    private void SetRate()
    {
        var module = shooterParticle.main;
        module.duration = StatSet[Stat.Rate];
    }

    protected override void PlaySFX()
    {
        base.PlaySFX();
        // Invoke("PlayExplosion", shooterParticle.main.startLifetime.constant);

    }


    private void PlayExplosion()
    {
        AudioManager.Main.RequestSFX(explosionSFX);
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
        string description = "releases a barrage of nukes for " + StatColorHandler.StatPaint(StatSet[Stat.Rate].ToString()) + " seconds that explodes in a small radius and deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage to all enemies hit";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " blast duration +25%";
        else return StatColorHandler.StatPaint("next level:") + " rate of fire +20%";
        
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 3 || toLevel == 5)
        {
            GainDuration();
        }
        else
        {
            GainRate();
        }
    }

    private void GainDuration()
    {
        var _duration = StatSet[Stat.Duration];
        _duration *= 1.25f;
        SetStat(Stat.Duration, _duration);
    }

    private void GainRate()
    {
        var _rate = StatSet[Stat.Rate];
        _rate *= 1.2f;
        SetStat(Stat.Rate, _rate);
    }
}
