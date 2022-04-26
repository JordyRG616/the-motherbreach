using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAction : BossAction
{
    private bool locked;
    private float counter;

    public override void Action()
    {
        
    }

    public override void DoActionMove()
    {
        if(locked) return;
        var direction = ship.position - transform.position;
        LookAt(direction);
    }

    public override void EndAction()
    {
        
    }

    public override void StartAction()
    {
        locked = false;
        if(controller.currentTrigger != "Special") controller.ActivateAnimation("Special");
        InitiateDelayedAttack();
    }

    public override void InitiateDelayedAttack()
    {
        locked = true;
        actionWeaponry.ForEach(x => x.Shoot());
    }
}
