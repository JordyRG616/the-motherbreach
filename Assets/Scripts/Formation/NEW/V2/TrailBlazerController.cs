using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBlazerController : FormationMovementController
{
    [SerializeField] private float splitTime = .4f;
    [SerializeField] private float splitCooldown;
    [SerializeField] private float distanceToSplit;
    private float orbitTimer;

    protected override void ManageStates()
    {

        if(orbitTimer >= splitCooldown)
        {
            RegisterMovement(EnemyMovementType.Split);
            var splitTimer = splitCooldown + splitTime;
            if(orbitTimer >= splitTimer)
            {
                orbitTimer = 0;
                RegisterMovement(EnemyMovementType.Orbit);
            }
        }
    }

    void Update()
    {
        orbitTimer += Time.deltaTime;
        Debug.Log(orbitTimer);
    }
}
