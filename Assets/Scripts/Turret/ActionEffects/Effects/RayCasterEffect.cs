using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class RayCasterEffect : ActionEffect
{
    [SerializeField] private float duration;
    [SerializeField] private float startSize;
    private FMOD.Studio.EventInstance instance;

    public override Stat specializedStat => Stat.Duration;

    public override Stat secondaryStat => Stat.Size;

    public override void SetData()
    {
        StatSet.Add(Stat.Duration, duration);
        StatSet.Add(Stat.Size, startSize);
        SetDuration();
        SetEmission();
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
        main.startLifetime = StatSet[Stat.Duration];
    }

    private void SetEmission()
    {
        var emission = shooterParticle.main;
        emission.startSize = StatSet[Stat.Size];
    }

    // public override void Shoot()
    // {
    //     // StartCoroutine(PlaySFX(StatSet[Stat.Duration]));
    //     AudioManager.Main.RequestSFX(onShootSFX, out sfxInstance);
    //     shooterParticle.Play();
    // }

    private IEnumerator PlaySFX(float duration)
    {
        AudioManager.Main.RequestSFX(onShootSFX, out var instance);

        yield return new WaitForSeconds(duration + 1);

        AudioManager.Main.StopSFX(instance);
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
        // var burned = hitManager.GetComponent<ChemicalBurn>();
        // if (burned == null) hitManager.gameObject.AddComponent<ChemicalBurn>();
    }

    public override string DescriptionText()
    {
        string description = "launches a growing area that deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage]) + " damage on contact and lasts for " + StatColorHandler.StatPaint(StatSet[Stat.Duration]) + " seconds";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " duration + 20%";
        else return StatColorHandler.StatPaint("next level:") + " shoots +2 rays per second";
        
    }

    public override void LevelUp(int toLevel)
    {
        var forceOverTime = shooterParticle.forceOverLifetime;
        forceOverTime.enabled = true;
        maxedOut = true;
    }

    public override void RemoveLevelUp()
    {
        if (!maxedOut) return;
        var forceOverTime = shooterParticle.forceOverLifetime;
        forceOverTime.enabled = false;
        maxedOut = false;
    }

    public override void RaiseInitialSpecializedStat(float percentage)
    {
        duration *= 1 + percentage;
    }

    public override void RaiseInitialSecondaryStat(float percentage)
    {
        startSize *= 1 + percentage;
    }
}
