using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class LeechEffect : ActionEffect
{
    [SerializeField] private float duration;
    [SerializeField] [Range(0, 1)] private float leech;
    [SerializeField] private float initalBulletSize;
    private FMOD.Studio.EventInstance instance;

    public override Stat specializedStat => Stat.Size;

    public override Stat secondaryStat => Stat.Duration;

    public override void SetData()
    {
        StatSet.Add(Stat.Duration, duration);
        SetDuration();
        StatSet.Add(Stat.Size, initalBulletSize);
        SetBulletSize();
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

    private void SetBulletSize()
    {
        var module = shooterParticle.sizeOverLifetime;
        module.sizeMultiplier = StatSet[Stat.Size];
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
        FindObjectOfType<ShipDamageController>().UpdateHealthNoEffects(StatSet[Stat.Damage] * leech);

    }

    public override string DescriptionText()
    {
        string description = "releases a barrage of shoots in a small area for " + StatColorHandler.StatPaint(StatSet[Stat.Duration].ToString()) + " seconds that deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage on contact and heals the ship for " + StatColorHandler.HealthPaint((StatSet[Stat.Damage] * leech).ToString());
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " duration +20%";
        else return StatColorHandler.StatPaint("next level:") + " heals +15%";
        
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 3 || toLevel == 5)
        {
            GainDuration();
        }
        else
        {
            GainLeech();
        }
    }

    private void GainDuration()
    {
        var _duration = StatSet[Stat.Duration];
        _duration *= 1.2f;
        SetStat(Stat.Duration, _duration);
    }

    private void GainLeech()
    {
        leech += 0.15f;
    }
}
