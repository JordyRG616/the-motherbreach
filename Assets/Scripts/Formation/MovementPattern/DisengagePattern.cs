using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisengagePattern : MovementPatternTemplate
{
    [SerializeField] private AnimationCurve curve;
    

    protected override IEnumerator DoMove()
    {

        Vector2 direction = transform.position - ship.position;

        float step = 0;

        Vector3 ogPosition = transform.position;

        while(step <= 1f)
        {
            Vector2 desloc = direction.normalized * curve.Evaluate(step);

            Rotate(-ship.position);

            transform.position = ogPosition + (Vector3)desloc * speed;

            step += 0.01f;

            yield return new WaitForSecondsRealtime(0.01f);

        }
    }

    private void Rotate(Vector3 target)
    {
        Vector3 direction = (target - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}
