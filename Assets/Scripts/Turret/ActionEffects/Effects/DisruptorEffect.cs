using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class DisruptorEffect : ActionEffect
{
    [SerializeField] private float disruptionDuration;
    private FMOD.Studio.EventInstance instance;
    [Range(0, 1)] [SerializeField] private float percentage;

    public override void SetData()
    {
        StatSet.Add(Stat.Duration, disruptionDuration);
        SetDuration();
        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetDuration();
    }

    private void SetDuration()
    {
        disruptionDuration = StatSet[Stat.Duration];
    }

    public override void Shoot()
    {
        // StartCoroutine(PlaySFX(StatSet[Stat.Duration]));
        AudioManager.Main.RequestSFX(onShootSFX, out sfxInstance);
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
        // hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
        ApplyStatusEffect<Expose>(hitManager, disruptionDuration, new float[] {percentage});
    }

    public override string DescriptionText()
    {
        string description = "releases a pulse of energy that applies " + StatColorHandler.StatPaint((percentage * 100).ToString()) + "% " + KeywordHandler.KeywordPaint(keyword) + " to each enemy hit and lasts for " + StatColorHandler.DamagePaint(StatSet[Stat.Duration].ToString()) + " seconds";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " expose duration + 20%";
        else return StatColorHandler.StatPaint("next level:") + " percentage + 10%";
        
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 3 || toLevel == 5)
        {
            GainDuration();
        }
        else
        {
            GainPercentage();
        }
    }

    private void GainDuration()
    {
        var _duration = StatSet[Stat.Duration];
        _duration *= 1.2f;
        SetStat(Stat.Duration, _duration);
    }

    private void GainPercentage()
    {
        percentage *= 1.1f;
    }
}
