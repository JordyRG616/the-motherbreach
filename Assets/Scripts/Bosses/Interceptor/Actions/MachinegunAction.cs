using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinegunAction : BossAction
{
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
        controller.ActivateAnimation("Attack", out var duration);
        Invoke("InitiateAttack", duration);
        LookAt(ship.position - transform.position);
    }

    private void InitiateAttack()
    {
        actionWeaponry.ForEach(x => x.Shoot());
    }
}
