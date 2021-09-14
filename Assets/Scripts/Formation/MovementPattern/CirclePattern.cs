using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePattern : MovementPatternTemplate
{

    [SerializeField] private float MinOrbitRadius, MaxOrbitRadius;
    

    void Start()
    {
        InvokeRepeating("Move", 0, .01f);
    }

    protected override void Move()
    {
        float distance = Vector3.Distance(transform.position, baseTransform.position);
         
        if(distance > MaxOrbitRadius || distance < MinOrbitRadius)
        {
            Seek(distance);
        } else
        {
            Orbit();
        }

        Rotate();
    }

    private void Rotate()
    {
        Vector3 direction = (baseTransform.position - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }

    private void Seek(float distance)
    {
        if(distance > MaxOrbitRadius)
        {
            Vector3 direction = (baseTransform.position - transform.position).normalized;
            transform.position += direction * speed/10;
        } else if(distance < MinOrbitRadius)
        {
            Vector3 direction = (baseTransform.position - transform.position).normalized;
            transform.position -= direction * speed;
        }
    }

    private void Orbit()
    {
        float angle = .01f * speed;
        float cos = Mathf.Cos(angle) - 1;
        float sin = Mathf.Sin(angle);
        float x = transform.position.x - baseTransform.position.x;
        float y = transform.position.y - baseTransform.position.y;

        Vector3 newPosition = new Vector3
        (
            x * cos - y * sin,
            y * cos + x * sin 
        );

        transform.position += newPosition;
    }
}
