using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FormationMovementController : MonoBehaviour
{
    protected delegate void Move(Rigidbody2D body, Vector3 target);

    [SerializeField] protected List<FormationMovement> movements;
    [SerializeField] EnemyMovementType attackingMovements;
    private EnemyMovementType currentMovement;
    protected Rigidbody2D body;
    protected ShipManager ship;
    protected Move doMove;
    protected float cooldown;

    protected virtual void Start()
    {
        body = GetComponent<Rigidbody2D>();
        ship = FindObjectOfType<ShipManager>();

        var move = movements[0];
        RegisterMovement(move.Type);
    }

    protected virtual void FixedUpdate()
    {
        cooldown += Time.fixedDeltaTime;
        if(cooldown >= 0.2f) cooldown = 0;
        if(cooldown == 0) ManageStates();
        doMove?.Invoke(body, ship.transform.position);
    }


    protected abstract void ManageStates();

    protected void RegisterMovement(EnemyMovementType movementType)
    {
        if(currentMovement == movementType) return;
        doMove = null;
        var movement =  movements.Find(x => x.Type == movementType);
        movement.Initiate();
        doMove = movement.DoMovement;
        currentMovement = movementType;
        if(attackingMovements.HasFlag(movementType)) StartCoroutine(OpenFire());
        else CeaseFire();
    }

    protected virtual IEnumerator OpenFire()
    {
        var enemies = GetComponentInParent<FormationManager>().Children;
        foreach(EnemyManager enemy in enemies)
        {
            enemy.OpenFire();
            yield return new WaitForSeconds(1f);
        }
    }
    
    private void CeaseFire()
    {
        var enemies = GetComponentInParent<FormationManager>().Children;
        foreach(EnemyManager enemy in enemies)
        {
            enemy.CeaseFire();
        }
    }

    public void AddSpeedModifier(float value)
    {
        foreach(FormationMovement movement in movements)
        {
            movement.speedModifier = value;
        }
    }
}
