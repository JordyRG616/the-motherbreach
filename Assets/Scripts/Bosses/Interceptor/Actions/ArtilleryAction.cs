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
        controller.ActivateAnimation("Attack", out var duration);
        Invoke("InitiateAttack", duration);
    }

    private void InitiateAttack()
    {
        actionWeaponry.ForEach(x => x.Shoot());
    }
}
