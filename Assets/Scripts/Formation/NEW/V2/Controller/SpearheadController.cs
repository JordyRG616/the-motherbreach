using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearheadController : FormationMovementController
{
    [SerializeField] private float distanceToAttack;
    [SerializeField] private float deltaDistance;
    [SerializeField] private float distanceToFlee;
    [SerializeField] private float durationOfFlee;
    private bool fleeing;

    protected override void Start()
    {
        base.Start();

        var rdm = Random.Range(-deltaDistance, deltaDistance);
        distanceToAttack += rdm;
    }

    protected override void ManageStates()
    {
        if(fleeing) return;
        var distance = Vector2.Distance(transform.position, ship.transform.position);
        if(distance <= distanceToAttack)
        {
            if(distance <= distanceToFlee)
            {
                RegisterMovement(EnemyMovementType.Flee);
                fleeing = true;
                Invoke("StopFleeing", durationOfFlee);
            } else
            {
                RegisterMovement(EnemyMovementType.Fixed);
            }
        }
        if(distance >= distanceToAttack + 5)
        {
            RegisterMovement(EnemyMovementType.Orbit);
        }
    }

    private void StopFleeing()
    {
        fleeing = false;
    }
}
