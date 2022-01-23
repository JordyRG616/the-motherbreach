using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableEntity : MonoBehaviour
{
    private Vector2 force;
    private Vector2 lastDirection;
    private float speed;
    [SerializeField] private float maxSpeed;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private enum MovingState {accelerating, slowing, inert, moving}
    private MovingState state = MovingState.inert;

    private IEnumerator _accelerate;
    private IEnumerator _slowDown;

    private bool anchored;
    private float drag;


    void Update()
    {
        if(anchored)
        {
            force = Vector2.zero;
        }
        transform.position += (Vector3)force * speed / 10 * Time.timeScale;
    }

    public void ApplyForce(Vector2 direction)
    {

        force += direction;
        force = Vector2.ClampMagnitude(force, 1);
        if(direction != Vector2.zero && direction == lastDirection)
        {
            if(state == MovingState.inert || state == MovingState.slowing)
            {
                // StopCoroutine(SlowDown());
                StopAllCoroutines();
                StartCoroutine(Accelerate());
            }
        }
        if(direction != lastDirection)//(direction == Vector2.zero)
        {

            if(state == MovingState.moving || state == MovingState.accelerating)
            {
                // StopCoroutine(Accelerate());
                StopAllCoroutines();
                StartCoroutine(SlowDown());
            }
        }

        lastDirection = direction;
    }

    internal void AccelerationOverride(object movement)
    {
        throw new NotImplementedException();
    }

    public void ApplyCrudeForce(Vector2 direction)
    {
        force += direction;
        // force = Vector2.ClampMagnitude(force, 1);
    }

    public void AccelerationOverride(Vector2 direction)
    {
        force += direction;
        force = Vector2.ClampMagnitude(force, 1);
        if(speed < maxSpeed) StartCoroutine(Accelerate());
    }

    public void SlowdownOverride()
    {
        StartCoroutine(SlowDown());
    }

    private IEnumerator Accelerate()
    {
        state = MovingState.accelerating;

        while(speed <= maxSpeed)
        {
            speed += 0.01f;

            yield return waitTime;
        }

        speed = maxSpeed;

        state = MovingState.moving;
    }

    private IEnumerator SlowDown()
    {
        state = MovingState.slowing;

        while(speed >= 0)
        {
            speed -= 0.01f;

            yield return waitTime;
        }

        speed = 0;

        state = MovingState.inert;
    }

    public void ClearForces()
    {
        force = Vector2.zero;
        StopAllCoroutines();
    }

    public void Anchor()
    {
        anchored = true;
    }

    public void LiftAnchor()
    {
        anchored = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, force * 5);
    }

    public void AddDrag(float percentage)
    {
        drag = percentage * maxSpeed;

        maxSpeed -= drag;
    }

    public void RemoveDrag()
    {
        maxSpeed += drag;
        drag = 0;
    }
}
