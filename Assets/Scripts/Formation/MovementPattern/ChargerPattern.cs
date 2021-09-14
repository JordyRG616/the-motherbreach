using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerPattern : MovementPatternTemplate
{
    [SerializeField] private float forkDistance;
    private Vector3 direction = Vector3.zero;


    void Start()
    {
        InvokeRepeating("Move", 0, .01f);
    }

    protected override void Move()
    {
        float distance = Vector3.Distance(transform.position, baseTransform.position);
        
        if(distance > forkDistance)
        {
            direction = (baseTransform.position - transform.position).normalized;
            Rotate();
        }

        Seek(direction);
    }

    private void Rotate()
    {
        Vector3 direction = - (baseTransform.position - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Seek(Vector3 direction)
    {
        transform.position += direction * speed/10;
    }
    
}
