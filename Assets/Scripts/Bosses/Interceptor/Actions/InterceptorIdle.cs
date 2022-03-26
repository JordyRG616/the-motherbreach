using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterceptorIdle : BossIdle
{
    private enum InterceptorMoveState {Follow, Disengage, Wiggle}
    private InterceptorMoveState moveState;

    [SerializeField] private float disengageSpeed;
    [SerializeField] private float wiggleSpeed;
    [SerializeField] private float distanceToFollow;
    [SerializeField] private float distanceToDisengage;
    [SerializeField] private float disengageDuration;
    [SerializeField] [FMODUnity.EventRef] private string movementSFX;
    private bool playingSFX;
    private FMOD.Studio.EventInstance SFXinstance;
    private float counter;
    private float distance;
    private Vector2 direction;
    private float _speed;
    private float RisingSpeed
    {
        get
        {
            if(_speed < speed) _speed += 0.05f;
            return _speed;
        }
    }


    public override void IdleMove()
    {
        direction = ship.position - transform.position;
        distance = Vector2.Distance(ship.position, transform.position);

        if(distance <= distanceToDisengage) Disengage();
        else if(distance >= distanceToFollow) Follow();
        else Wiggle();

        LookAt(direction);
    }

    private void Wiggle()
    {
        if(moveState == InterceptorMoveState.Disengage) return;
        body.velocity = Vector2.zero;
        var perpendicular = Vector2.Perpendicular(direction).normalized;
        perpendicular *= Mathf.Sin(Time.time);

        body.AddForce(perpendicular * wiggleSpeed, ForceMode2D.Impulse);

        if(moveState != InterceptorMoveState.Wiggle)
        {
            moveState = InterceptorMoveState.Wiggle;
        }
    }

    private void Disengage()
    {
        // body.velocity = Vector2.zero;
        var perpendicular = Vector2.Perpendicular(direction);
        perpendicular +=  perpendicular - direction;
        body.AddForce(perpendicular.normalized * disengageSpeed, ForceMode2D.Impulse);
        
        if(moveState != InterceptorMoveState.Disengage)
        {
            // Invoke("StopDisengage", disengageDuration);
            moveState = InterceptorMoveState.Disengage;
        }
    }

    private void Follow()
    {
        if(moveState == InterceptorMoveState.Disengage) return;
        
        if(moveState != InterceptorMoveState.Follow)
        {
            _speed = 0;
            moveState = InterceptorMoveState.Follow;
        }

        body.velocity = Vector2.zero;
        var perpendicular = Vector2.Perpendicular(direction) * Mathf.Sin(Time.timeSinceLevelLoad);
        var _direction = perpendicular + direction;
        body.AddForce(_direction.normalized * RisingSpeed, ForceMode2D.Impulse);

    }

    void FixedUpdate()
    {    
        if(moveState != InterceptorMoveState.Disengage) return;

        counter += Time.fixedDeltaTime;
        if(counter >= disengageDuration) StopDisengage();
    }

    private void StopDisengage()
    {
        moveState = InterceptorMoveState.Follow;
        counter = 0;
    }

}
