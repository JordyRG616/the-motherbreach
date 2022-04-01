using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrageController : FormationMovementController
{
    protected override void ManageStates()
    {
        // var distance = Vector2.Distance(ship.transform.position, transform.position);
        RegisterMovement(EnemyMovementType.Hunt);
    }
}
