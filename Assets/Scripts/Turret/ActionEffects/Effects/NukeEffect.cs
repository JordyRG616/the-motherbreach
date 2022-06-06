using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class NukeEffect : ActionEffect
{
    [SerializeField] private float blastSize;
    [SerializeField] private float initalBurst;
    [SerializeField] private ParticleSystem subEmitter;
    [SerializeField] [FMODUnity.EventRef] private string explosionSFX;
    private FMOD.Studio.EventInstance instance;
    private int bombCount = 3;

    public override Stat specializedStat => Stat.Size;

    public override Stat secondaryStat => Stat.BurstCount;

    public override void SetData()
    {
        StatSet.Add(Stat.BurstCount, initalBurst);
        StatSet.Add(Stat.Size, blastSize);
        SetBurstCount();
        SetBlastSize();
        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetBurstCount();
        SetBlastSize();
    }

    private void SetBlastSize()
    {
        var shape = subEmitter.shape;
        shape.radius = StatSet[Stat.Size];
    }

    private void SetBurstCount()
    {
        var module = shooterParticle.emission;
        var burst = module.GetBurst(0);

        burst.cycleCount = Mathf.CeilToInt(StatSet[Stat.BurstCount]);
        module.SetBurst(0, burst);
    }

    protected override void PlaySFX()
    {
        base.PlaySFX();
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
        //hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    public override string DescriptionText()
    {
        string description = "releases " + StatColorHandler.StatPaint(StatSet[Stat.BurstCount]) + " barrages of " + bombCount + " nukes that explodes in a small radius and deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage to all enemies hit";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " blast duration +25%";
        else return StatColorHandler.StatPaint("next level:") + " rate of fire +20%";
        
    }

    public override void LevelUp(int toLevel)
    {
        var emission = shooterParticle.emission;
        var burst = emission.GetBurst(0);
        burst.count = new ParticleSystem.MinMaxCurve(burst.count.constant + 1);
        bombCount += 1;
        emission.SetBurst(0, burst);

        var main = subEmitter.main;
        main.startLifetime = new ParticleSystem.MinMaxCurve(main.startLifetime.constant + 0.5f);
        maxedOut = true;
    }

    public override void RemoveLevelUp()
    {
        if (!maxedOut) return;

        var emission = shooterParticle.emission;
        var burst = emission.GetBurst(0);
        burst.count = new ParticleSystem.MinMaxCurve(burst.count.constant - 1);
        bombCount -= 1;
        emission.SetBurst(0, burst);

        var main = subEmitter.main;
        main.startLifetime = new ParticleSystem.MinMaxCurve(main.startLifetime.constant - 0.5f);
    }

    public override void RaiseInitialSpecializedStat(float percentage)
    {
        blastSize *= 1 + percentage;
    }

    public override void RaiseInitialSecondaryStat(float percentage)
    {
        initalBurst *= 1 + percentage;
    }
}
 