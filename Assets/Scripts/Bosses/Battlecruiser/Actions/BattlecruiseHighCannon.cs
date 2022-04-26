using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlecruiseHighCannon : BossAction
{

    public override void Action()
    {
        
    }

    public override void DoActionMove()
    {
        // if(locked) return;
        // var direction = ship.position - transform.position;
        // LookAt(direction);
    }

    public override void EndAction()
    {
        actionWeaponry.ForEach(x => x.StopShooting());
    }

    public override void StartAction()
    {
        if(controller.currentTrigger != "Special") controller.ActivateAnimation("Special");
        // InitiateDelayedAttack();
    }

    public override void InitiateDelayedAttack()
    {
        actionWeaponry.ForEach(x => x.Shoot());
    }
}
