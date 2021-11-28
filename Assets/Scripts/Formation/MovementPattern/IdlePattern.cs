using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePattern : MovementPatternTemplate
{
    [SerializeField] [Range(0, 1f)] private float variationFactor;
    [SerializeField] private float idleDuration;
    private int[] rotateDirections = new int[2] {-1, 1};
    
    protected override IEnumerator DoMove()
    {
        float step = 0f;

        int rotateDir = rotateDirections[Random.Range(0, 2)];

        Debug.Log(rotateDir);

        while(step <= idleDuration)
        {
            Vector2 rdmDirection = ship.position - transform.position;
            rdmDirection = Vector2.Perpendicular(rdmDirection * rotateDir).normalized;

            float variation = Random.Range(-variationFactor, variationFactor);

            rdmDirection += new Vector2(variation, variation);
            
            Rotate(ship.position, rotateDir);

            transform.position += (Vector3)rdmDirection * speed;

            step += 0.01f;
            
            yield return new WaitForSecondsRealtime(0.01f);
        }

        transform.localScale = new Vector2 (1, 1);
    }

    private void Rotate(Vector3 target, int rotateDir)
    {
        Vector3 direction = (target - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.localScale = new Vector2(transform.localScale.x, rotateDir);

        transform.rotation = Quaternion.Euler(0, 0, angle + 180f);
    }
}
