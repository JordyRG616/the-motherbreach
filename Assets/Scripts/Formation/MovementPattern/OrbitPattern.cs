using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitPattern : MovementPatternTemplate
{

    [SerializeField] private float orbitDuration;

    private void Rotate(Vector3 target)
    {
        Vector3 direction = (target - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }

    protected override IEnumerator DoMove()
    {
        float step = 0f;

        while(step <= orbitDuration)
        {
            Rotate(ship.position);

            float angle = .01f * speed;
            float cos = Mathf.Cos(angle) - 1;
            float sin = Mathf.Sin(angle);
            float x = transform.position.x - ship.position.x;
            float y = transform.position.y - ship.position.y;

            Vector3 newPosition = new Vector3
            (
                x * cos - y * sin,
                y * cos + x * sin 
            );

            transform.position += newPosition;

            step += 0.01f;

            yield return new WaitForSeconds(0.01f);
        }
    }
}
