using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    private Transform target;
    [SerializeField] private Vector2 speedRange;
    private float speed;
    [SerializeField] private Vector2 majorRadiusRange;
    private float majorR;
    [SerializeField] private Vector2 minorRadiusRange;
    private float minorR;
    private WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(0.01f);

    void Awake()
    {
        Configure();
    }

    public void StartMoving()
    {
        StartCoroutine(Move());
    }

    public void StopMoving()
    {
        StopAllCoroutines();
    }

    private IEnumerator Move()
    {
        float step = 0;

        while(true)
        {
            transform.position += (Vector3)ReturnPosition(step) / 50;

            step += 1f;

            yield return waitTime;
        }
    }
    private void Configure()
    {
        target =  GetComponentInParent<Transform>();

        speed = UnityEngine.Random.Range(speedRange.x, speedRange.y);

        majorR = UnityEngine.Random.Range(majorRadiusRange.x, majorRadiusRange.y);

        minorR = UnityEngine.Random.Range(minorRadiusRange.x, minorRadiusRange.y);

    }

    private Vector2 ReturnPosition(float step)
    {
        // float x = target.position.x + (majorR * Mathf.Cos((step * speed) * Mathf.Deg2Rad));
        float x = (majorR * Mathf.Cos((step * speed) * Mathf.Deg2Rad));

        // float y = target.position.y + (minorR * Mathf.Sin((step * speed) * Mathf.Deg2Rad));
        float y = (minorR * Mathf.Sin((step * speed) * Mathf.Deg2Rad));


        return new Vector2(x, y);
    }

}
