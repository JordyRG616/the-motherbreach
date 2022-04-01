using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerPattern : MovementPatternTemplate
{

    [SerializeField] private float radius;
    private float distance;

    private void Rotate(Vector3 target)
    {
        Vector3 direction = (target - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }

    protected override IEnumerator DoMove()
    {
        distance = Vector2.Distance(ship.position, transform.position);          

        while(distance > radius)
        {
            Vector3 direction = (ship.position - transform.position).normalized;
            Rotate(ship.position);
            transform.position += direction * speed / 2;

            
            distance = Vector2.Distance(ship.position, transform.position);          

            yield return new WaitForSeconds(0.01f);
        }
    }
}
