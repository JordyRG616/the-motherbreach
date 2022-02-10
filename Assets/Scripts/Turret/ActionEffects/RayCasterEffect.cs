using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class RayCasterEffect : ActionEffect
{
    [SerializeField] private float duration;
    [SerializeField] private int emission;
    private FMOD.Studio.EventInstance instance;

    public override void SetData()
    {
        StatSet.Add(Stat.Duration, duration);
        StatSet.Add(Stat.Projectiles, emission);
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
        main.duration = StatSet[Stat.Duration];
    }

    private void SetEmission()
    {
        var emission = shooterParticle.emission;
        emission.rateOverDistance = StatSet[Stat.Projectiles];
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
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
        // var burned = hitManager.GetComponent<ChemicalBurn>();
        // if (burned == null) hitManager.gameObject.AddComponent<ChemicalBurn>();
    }

    public override string DescriptionText()
    {
        string description = "releases a stream of" + StatColorHandler.StatPaint(StatSet[Stat.Projectiles].ToString()) + " rays for " + StatColorHandler.StatPaint(StatSet[Stat.Duration].ToString()) + " seconds that deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage on contact and apply";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " duration + 20%";
        else return StatColorHandler.StatPaint("next level:") + " shoots +2 rays per second";
        
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 3 || toLevel == 5)
        {
            GainDuration();
        }
        else
        {
            GainRay();
        }
    }

    private void GainDuration()
    {
        var _duration = StatSet[Stat.Duration];
        _duration *= 1.2f;
        SetStat(Stat.Duration, _duration);
    }

    private void GainRay()
    {
        var projectiles = StatSet[Stat.Projectiles];
        projectiles += 2f;
        SetStat(Stat.Projectiles, projectiles);
    }
}
