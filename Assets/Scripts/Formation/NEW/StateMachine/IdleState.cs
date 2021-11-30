using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : FormationState
{
    private Transform target;
    [SerializeField] private Vector2 speedRange;
    private float speed;
    [SerializeField] private Vector2 majorRadiusRange;
    private float majorR;
    [SerializeField] private Vector2 minorRadiusRange;
    private float minorR;
    private bool configured = false;
    [SerializeField] private ParticleSystem rift;


    private WaitForSecondsRealtime waitStep = new WaitForSecondsRealtime(0.01f);

    private void Configure()
    {
        target = FindObjectOfType<ShipManager>().transform;

        speed = UnityEngine.Random.Range(speedRange.x, speedRange.y);
        float sign = Mathf.Sign(UnityEngine.Random.Range(-1, 1));
        speed *= sign;

        majorR = UnityEngine.Random.Range(majorRadiusRange.x, majorRadiusRange.y);

        minorR = UnityEngine.Random.Range(minorRadiusRange.x, minorRadiusRange.y);

        step = GetStepValue(transform.position.x, transform.position.y);

        if((Vector2)transform.position != ReturnPosition(step))
        {
            StartCoroutine(EnterOrbit(transform.position, ReturnPosition(step)));
        }

        configured = true;
    }

    private IEnumerator EnterOrbit(Vector2 origin, Vector2 target)
    {
        LookAtShip();

        rift.Play();

        float _step = 0;

        while(_step <= 1)
        {
            Vector2 newPos = Vector2.Lerp(origin, target, _step);

            transform.position = newPos;

            _step += 0.01f;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        rift.Stop();

        OnStateEnter();
    }

    public override void Enter()
    {
        if(!configured)
        {
            Configure();
        } else
        {
            OnStateEnter();
        }
            

    }

    protected override void OnStateEnter()
    {
        foreach (EnemyRotationController enemy in GetComponentsInChildren<EnemyRotationController>())
        {
            enemy.StartRotation(Mathf.Sign(speed));
            enemy.GetComponent<EnemyMovementController>().StartMoving();
        }

        base.OnStateEnter();
    }

    protected override void OnStateTick(float step)
    {
        transform.position = ReturnPosition(step);
        LookAtShip();
    }

    private Vector2 ReturnPosition(float step)
    {
        float x = target.position.x + (majorR * Mathf.Cos((step * speed) * Mathf.Deg2Rad));
        float y = target.position.y + (minorR * Mathf.Sin((step * speed) * Mathf.Deg2Rad));

        return new Vector2(x, y);
    }

    public void LookAtShip()
    {
        Vector2 direction = (target.position - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private float GetStepValue(float xPos, float yPos)
    {
        float arg = (xPos - target.position.x)/Vector2.Distance(target.position, new Vector2(xPos, yPos));
        Debug.Log(arg);
        float stepX = Mathf.Acos(arg) * Mathf.Rad2Deg / speed;
        float _y = Mathf.Sign(target.position.y + (minorR * Mathf.Sin((stepX * speed) * Mathf.Deg2Rad)));

        if(Mathf.Sign(yPos) != _y)
        {
            transform.position = new Vector3(xPos, -yPos);
        }

        return stepX;
    }

    protected override void OnStateExit()
    {
        foreach(EnemyRotationController enemy in GetComponentsInChildren<EnemyRotationController>())
        {
            enemy.StopRotation();
            enemy.GetComponent<EnemyMovementController>().StopMoving();
        }

        base.OnStateExit();
    }
    
    public void SetSpeed(float value)
    {
        speed = value;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
