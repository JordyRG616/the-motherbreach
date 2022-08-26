using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortableShield : Deployable
{
    [Header("Specifics")]
    [SerializeField] private float launchSpeed;
    [SerializeField] private float orbitSpeed;
    [SerializeField] private Vector2 originPoint;
    [SerializeField] private ParticleSystem VFX;
    [SerializeField] private float directionModifier;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string onRedockSFX;
    private Vector2 direction;
    private CircleCollider2D coll;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    private DeployerWeapon deployer;
    private Transform shipManager;
    private float maxDistance;

    private void Awake()
    {
        deployer = GetComponentInParent<DeployerWeapon>();
        deployer.ReceiveDeployable(this);

        OnRedock += StopRoutines;
        direction = transform.position - transform.parent.position;
        direction.Normalize();

        shipManager = ShipManager.Main.transform;
        coll = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        spriteRenderer.color = Color.clear;
    }

    protected override void Update()
    {
        base.Update();
        if (deployer == null) Destroy(gameObject);
    }

    public override void Initialize()
    {
        deployer.SetDeployPoint(originPoint);
    }

    public override void Launch()
    {
        StartCoroutine(LaunchSequence());
        VFX.Play();
        coll.enabled = true;
    }

    private void StopRoutines(Deployable deployable)
    {
        StopAllCoroutines();
    }

    private IEnumerator LaunchSequence()
    {
        direction = transform.parent.position - shipManager.position;
        direction += Vector2.Perpendicular(direction) / directionModifier;
        direction.Normalize();
        float step = 0;
        var color = Color.white;

        while (step <= 1)
        {
            transform.position += (Vector3)direction * launchSpeed * (1 - step);
            color.a = step;
            spriteRenderer.color = color;

            step += 0.01f;
            yield return waitTime;
        }


        spriteRenderer.color = Color.white;
        StartCoroutine(Orbit());
    }

    private IEnumerator Orbit()
    {
        maxDistance = Vector2.Distance(transform.position, shipManager.position);
        transform.SetParent(null);

        while (true)
        {
            direction = shipManager.position - transform.position;
            var perpendicular = Vector2.Perpendicular(direction);
            if (direction.magnitude > maxDistance) perpendicular += direction * -directionModifier;
            direction = perpendicular.normalized;

            transform.position += (Vector3)direction * orbitSpeed * -directionModifier;

            yield return new WaitForEndOfFrame();
            //yield return waitTime;
        }
    }

    public void SetSize(float value)
    {
        coll.radius = value;
        var shape = VFX.shape;
        shape.radius = value;
        spriteRenderer.transform.localScale = value * 0.6f * Vector3.one;
    }

    public void SetOrbitSpeed(float value)
    {
        orbitSpeed = value;
    }

    protected override void DisableDeployable()
    {
        StopCoroutine(Orbit());
        VFX.Stop();
        spriteRenderer.color = Color.clear;
        coll.enabled = false;
        transform.SetParent(deployer.transform);
        StartCoroutine(ReturnToTurret());
    }

    private IEnumerator ReturnToTurret()
    {
        float step = 0;
        var location = transform.localPosition;

        while (step <= 1)
        {
            var pos = Vector3.Lerp(location, originPoint, step);
            transform.localPosition = pos;

            step += 0.01f;
            yield return waitTime;
        }

        transform.localPosition = originPoint;
        transform.rotation = transform.parent.rotation;
        AudioManager.Main.RequestSFX(onRedockSFX);
        OnRedock?.Invoke(this);
        remainingLifetime = lifetime;
    }
}
