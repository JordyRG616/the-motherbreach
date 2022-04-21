using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : ActionController
{
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float orbitRadius;

    [SerializeField] private Vector3 targetSize;
    private Vector3 ogSize;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private bool tracking;
    private Rigidbody2D body;
    private Vector2 direction;
    private Transform ship;


    void Awake()
    {
        Initiate();
        GetComponent<DeployableObject>().onBirth += Grow;
        GetComponent<DeployableObject>().onLifetimeEnd += ResetSize;

        ogSize = transform.localScale;
    }

    private void ResetSize(object sender, EventArgs e)
    {
        transform.localScale = new Vector3(0, 0, 1);
    }

    private void Grow(object sender, EventArgs e)
    {
        Debug.Log("called");
        StartCoroutine(GrowToFullSize());
    }

    private IEnumerator GrowToFullSize()
    {
        float step = 0;

        while(step <= 1)
        {
            var size = Vector3.Lerp(ogSize, targetSize, step);
            transform.localScale = size;

            step += 0.1f;
            yield return waitTime;
        }
    }

    public override void Initiate()
    {
        base.Initiate();

        body = GetComponent<Rigidbody2D>();
        ship = ShipManager.Main.transform;
    }

    public override void Activate()
    {
        foreach(ActionEffect shooter in shooters)
        {
            if(shooter.GetShooterSystem().IsAlive()) return;
            shooter.Shoot();
        }
    }

    protected override IEnumerator ManageActivation()
    {
        yield break;
    }

    private void TrackTarget()
    {
        direction = target.transform.position - transform.position;

        if(direction.magnitude > range)
        {
            body.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
        }
    }

    private void OrbitShip()
    {
        direction = Vector2.Perpendicular(direction);

        body.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
    }

    private void MoveAwayFromShip()
    {
        direction = -direction;
        body.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
    }
    
    private void RotateTowards(Vector2 direction)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    protected override void FixedUpdate()
    {
        body.velocity = Vector2.zero;

        if(target == null)
        {
            direction = ship.position - transform.position;

            if(direction.magnitude < orbitRadius) MoveAwayFromShip();
            else OrbitShip();
         
        } else TrackTarget();


        RotateTowards(direction);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if(enemiesInSight.Count == 0 || target != null) return;
        target = enemiesInSight[0];

        foreach(ActionEffect shooter in shooters)
        {
            shooter.ReceiveTarget(target.gameObject);
        }

        Invoke("Activate", 1f);

    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);

        if(target != null && !enemiesInSight.Contains(target))
        {
            target = null;
            StopShooters();
        } 
    }
}
