using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CaterpillarController : FormationMovementController
{
    [SerializeField] private float distanceToOrbit;
    private Transform target;

    protected override void ManageStates()
    {
        if(target == null)
        {
            GetTarget();
        } 
        var distance = Vector2.Distance(target.transform.position, transform.position);
        RegisterMovement(EnemyMovementType.Hunt);
        // if(distance <= distanceToOrbit)
        // {
        //     RegisterMovement(EnemyMovementType.TrueOrbit);
        // }
    }

    private void GetTarget()
    {
        var formations = FindObjectsOfType<FormationMovementController>().ToList();
        formations.Remove(this);

        formations.OrderBy(x => Vector2.Distance(x.transform.position, transform.position));

        target = formations.First().transform;
    }
    
    protected override void FixedUpdate()
    {
        if(cooldown >= 0.2f) cooldown = 0;
        if(cooldown == 0) ManageStates();
        if(target == null) return;
        cooldown += Time.fixedDeltaTime;
        doMove?.Invoke(body, target.transform.position);
    }
}
