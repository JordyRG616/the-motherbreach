using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class MercenaryEffect : BaseEffectTemplate
{
    [SerializeField] private ParticleSystem vfx;
    [SerializeField] [FMODUnity.EventRef] private string sfx;
    private EnemyDeathEvent deathEvent;
    public int count;

    public override void Initiate()
    {
        deathEvent = GetComponent<EnemyDeathEvent>();
        deathEvent.effect += ApplyEffect;
        base.Initiate();
    }

    public override void ApplyEffect()
    {
        if(deathEvent.killer == associatedController.gameObject)
        {
            count++;
            RewardManager.Main.EarnCash(1);
            if(gameManager.gameState == GameState.OnReward) return;
            vfx.Play();
            AudioManager.Main.RequestGUIFX(sfx);
        }
    }

    public override string DescriptionText()
    {
        return "receive " + StatColorHandler.StatPaint(1.ToString()) + " cash if the enemy was killed by this turret";
    }
}
