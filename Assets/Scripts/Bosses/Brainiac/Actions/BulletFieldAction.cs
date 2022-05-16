using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFieldAction : BossAction
{
    [SerializeField] private List<ParticleSystem> spinners;
    [SerializeField] [FMODUnity.EventRef] private string openSFX;
    [SerializeField] [FMODUnity.EventRef] private string closeSFX;
    private float counter;

    public override void Action()
    {
        
    }

    public override void DoActionMove()
    {

    }

    public override void EndAction()
    {
        controller.ActivateAnimation("Move");
        spinners.ForEach(x => x.Stop());
        AudioManager.Main.RequestSFX(closeSFX);
        actionWeaponry.ForEach(x => x.StopShooting());
    }

    public override void StartAction()
    {
        if(controller.currentTrigger == "Attack") InitiateDelayedAttack();
        else controller.ActivateAnimation("Attack");
        AudioManager.Main.RequestSFX(openSFX);
    }

    public override void InitiateDelayedAttack()
    {
        spinners.ForEach(x => x.Play(false));
        actionWeaponry.ForEach(x => x.Shoot());
    }
}
