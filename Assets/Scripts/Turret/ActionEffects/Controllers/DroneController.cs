using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : ActionController
{
    private DroneMovement movement;
    private WaitForSeconds waitTime;

    public void StartComponent()
    {
        StartCoroutine(ManageActivation());
    }

    public void Configure(float level)
    {
        movement = GetComponent<DroneMovement>();

        Initiate();

        GetTarget();

        var damage = shooters[0].StatSet[Stat.Damage];
        shooters[0].SetStat(Stat.Damage, damage * level);
        var rest = shooters[0].StatSet[Stat.Rest];
        shooters[0].SetStat(Stat.Rest, rest + (level/10));

        waitTime = new WaitForSeconds(shooters[0].StatSet[Stat.Rest]);
    }

    public override void Activate()
    {
        foreach(ActionEffect shooter in shooters)
        {
            shooter.Shoot();
        }
    }

    public void GetTarget()
    {
        if(movement.GetTarget() != null)
        {
            target = movement.GetTarget().GetComponent<EnemyManager>();
            shooters[0].ReceiveTarget(target.gameObject);
        } else
        {
            Stop();
        }
    }

    protected override IEnumerator ManageActivation()
    {
        while(target != null)
        {
            yield return new WaitUntil(() => Vector2.Distance(transform.position, target.transform.position) <= movement.GetDistance());

            Activate();
        }
    }

    public void Stop()
    {
        StopAllCoroutines();
        StopShooters();
    }
}
