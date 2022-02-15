using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Artifact : MonoBehaviour
{
    public EffectTrigger trigger;
    public Sprite icon;

    protected abstract void Effect();
    public abstract string Description();

    public void Initialize()
    {
        HandleEffect();
    }

    private void HandleEffect()
    {
        switch(trigger)
        {
            case EffectTrigger.Immediate:
                Effect();
                break;
            case EffectTrigger.StartOfWave:
                GameManager.Main.OnGameStateChange += HandleStateEffect;
                break;
            case EffectTrigger.EndOfWave:
                GameManager.Main.OnGameStateChange += HandleStateEffect;
                break;
            case EffectTrigger.OnDestruction:
                foreach(TurretManager turret in ShipManager.Main.turrets)
                {
                    turret.GetComponent<HitManager>().OnDeath += HandleCommonEffect;
                }
                break;
            case EffectTrigger.OnLevelUp:
                foreach(TurretManager turret in ShipManager.Main.turrets)
                {
                    turret.OnLevelUp += HandleLevelEffect;
                }
                break;
            case EffectTrigger.OnHit:
                foreach(TurretManager turret in ShipManager.Main.turrets)
                {
                    turret.GetComponent<HitManager>().OnHit += HandleHitEffect;
                }
                break;
            case EffectTrigger.OnTurretBuild:
                RewardManager.Main.OnTurretBuild += HandleCommonEffect;
                break;
            case EffectTrigger.OnTurretSell:
                FindObjectOfType<SellButton>().OnTurretSell += HandleCommonEffect;
                break;
            case EffectTrigger.OnEnemyDefeat:
                GetComponent<EnemyDeathEvent>().effect += Effect;
                break;
        }
    }

    protected virtual void HandleHitEffect(object sender, HitEventArgs e)
    {
        Effect();
    }

    protected virtual void HandleLevelEffect(object sender, LevelUpArgs e)
    {
        Effect();
    }

    protected virtual void HandleCommonEffect(object sender, EventArgs e)
    {
        Effect();
    }

    protected virtual void HandleStateEffect(object sender, GameStateEventArgs e)
    {
        if(e.effectTrigger == trigger)
        {
            Effect();
        }
    }
}
