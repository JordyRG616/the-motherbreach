using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainiacIdle : BossIdle
{
    [SerializeField] private float distanceToTeleport;
    private TeleportAction teleportAction;
    private Vector2 direction;

    void Awake()
    {
        teleportAction = GetComponent<TeleportAction>();
    }

    public override void IdleMove()
    {
        var distance = Vector2.Distance(ship.position, transform.position);

        if(distance <= distanceToTeleport && !controller.onAction)
        {
            teleportAction.InitiateTeleport();
        } else
        {
            Wiggle();
        }
    }

    private void Wiggle()
    {
        direction = Random.onUnitSphere;
        var _d = ship.position - transform.position;
        
        body.AddForce(direction * speed);

        LookAt(_d);
    }
}
