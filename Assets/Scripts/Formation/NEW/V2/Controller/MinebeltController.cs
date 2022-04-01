using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinebeltController : FormationMovementController
{
    [SerializeField] private int distanceToAttack;
    [SerializeField] private int distanceOfCharge;

    protected override void ManageStates()
    {
        var distance = Vector2.Distance(transform.position, ship.transform.position);
        if(distance <= distanceToAttack)
        {
            RegisterMovement(EnemyMovementType.Charge);
        }
        else if(distance >= distanceToAttack + distanceOfCharge)
        {
            RegisterMovement(EnemyMovementType.Orbit);
        }
    }


}
