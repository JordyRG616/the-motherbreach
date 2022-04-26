using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFieldAction : BossAction
{
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
        actionWeaponry.ForEach(x => x.StopShooting());
    }

    public override void StartAction()
    {
        if(controller.currentTrigger == "Attack") InitiateDelayedAttack();
        else controller.ActivateAnimation("Attack");
    }

    public override void InitiateDelayedAttack()
    {
        actionWeaponry.ForEach(x => x.Shoot());
    }
}
