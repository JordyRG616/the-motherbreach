using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    [SerializeField] private float lifetime;
    private WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(0.01f);
    public bool deployed {get; private set;}

    public void StartMoving()
    {
        deployed = true;

        StartCoroutine(Move());
        StartCoroutine(Live());
    }

    private IEnumerator Live()
    {
        yield return new WaitForSecondsRealtime(lifetime);

        StopMoving();
        GetComponent<DroneController>().Stop();
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void StopMoving()
    {
        StopAllCoroutines();
    }

    private IEnumerator Move()
    {
        float step = 0;
        float increment = 0;
        Vector2 outPos = new Vector2(0, 1.5f);

        while(step <= 1)
        {
            transform.localPosition = Vector2.Lerp(Vector2.zero, outPos, step);
            increment += 0.001f;
            step += increment;
            
            yield return waitTime;
        }

        step = 0;

        while(true)
        {
            if(Vector2.Distance(target.transform.position, transform.position) > distance)
            {
                transform.position += (Vector3)ReturnFollowPosition() * speed / 100;
            }

            if(Vector2.Distance(target.transform.position, transform.position) <= distance)
            {
                transform.position += (Vector3)ReturnOrbitPosition() * speed / 100;
            }

            Rotate();

            step += 1f;

            yield return waitTime;
        }
    }

    public void Configure(Transform target, float level)
    {
        this.target = target;
        lifetime += level * 2;
    }

    private Vector2 ReturnFollowPosition()
    {
        Vector2 direction = target.transform.position - transform.position;

        return direction.normalized;
    }

    private Vector2 ReturnOrbitPosition()
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        direction = Vector2.Perpendicular(direction);

        return direction;

    }

    private void Rotate()
    {
        Vector2 direction = (target.position - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    public Transform GetTarget()
    {
        return target;
    }

    public float GetDistance()
    {
        return distance;
    }
}
