using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinegunAction : BossAction
{
    [SerializeField] [FMODUnity.EventRef] private string chargeUpSFX;
    private float counter;

    public override void Action()
    {
        // counter += Time.fixedDeltaTime;

        // if(counter >= actionDuration) EndAction();
    }

    public override void DoActionMove()
    {
        body.velocity = Vector2.zero;
    }

    public override void EndAction()
    {
        actionWeaponry.ForEach(x => x.StopShooting());
    }

    public override void StartAction()
    {
        if(controller.currentTrigger == "Attack") InitiateDelayedAttack();
        else controller.ActivateAnimation("Attack");
        LookAt(ship.position - transform.position);
    }

    public override void InitiateDelayedAttack()
    {
        AudioManager.Main.RequestSFX(chargeUpSFX);
        actionWeaponry.ForEach(x => x.Shoot());
    }
}
