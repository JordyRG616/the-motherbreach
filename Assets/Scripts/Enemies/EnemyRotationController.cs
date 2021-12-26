using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotationController : MonoBehaviour
{
    private Transform ship;
    private IdleState reference;
    private float sign = 1;

    void Awake()
    {
        ship = FindObjectOfType<ShipManager>().transform;
    }

    private IEnumerator Rotate()
    {
        while(true)
        {
            Vector2 direction = (ship.position - transform.position);
            direction = Vector2.Perpendicular(direction);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle + 90f * sign);

            yield return new WaitForEndOfFrame();
        }
    }

    public void LookAtShip(float sign = 1)
    {
        Vector2 direction = (ship.position - transform.position) * sign;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    public void LookAt(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void StartRotation(float sign)
    {
        this.sign = sign;
        StartCoroutine(Rotate());
    }

    public void StopRotation()
    {
        StopAllCoroutines();
    }

}
