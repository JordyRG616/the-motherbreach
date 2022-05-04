using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryAction : BossAction
{
    private float counter;

    public override void Action()
    {
        // counter += Time.fixedDeltaTime;

        // if(counter >= actionDuration) EndAction();
    }

    public override void DoActionMove()
    {

    }

    public override void EndAction()
    {
        actionWeaponry.ForEach(x => x.StopShooting());
    }

    public override void StartAction()
    {
        if(controller.currentTrigger == "Attack") InitiateDelayedAttack();
        else controller.ActivateAnimation("Attack");

        controller.StopMovementSFX();
    }

    public override void InitiateDelayedAttack()
    {
        actionWeaponry.ForEach(x => x.Shoot());
    }
}
