using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerPattern : MovementPatternTemplate
{
    [SerializeField] private float correctionAngle;
    [SerializeField] private float chargeDuration;

    protected override IEnumerator DoMove()
    {
        Vector2 distance = (Vector2)(ship.position - transform.position);
        distance = new Vector2(distance.x * Mathf.Cos(correctionAngle * Mathf.Deg2Rad), distance.y * Mathf.Sin(correctionAngle * Mathf.Deg2Rad));

        Rotate(distance);

        float step = 0f;

        while(step <= chargeDuration)
        {
            
            transform.position += (Vector3)distance.normalized * speed;

            step += 0.01f;

            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    private void Rotate(Vector3 target)
    {
        Vector3 direction = (target - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }
    
}
