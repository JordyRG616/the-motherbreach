using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherArrow : Deployable
{
    [SerializeField] private float launchSpeed;
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem trail;
    [SerializeField] private TurretActionMediator actionMediator;
    private DeployerWeapon weapon;
    private WaitForSeconds time = new WaitForSeconds(0.01f);
    private Vector2 direction = Vector2.up;
    private Transform ship;

    private void Start()
    {
        ship = ShipManager.Main.transform;
        weapon = GetComponentInParent<DeployerWeapon>();
        weapon.ReceiveDeployable(this);
        actionMediator.linkedWeapon = weapon;
        weapon.totalEffect += ApplyTrap;
        gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().color = new Color(0, 0.9301112f, 0.9911022f);
    }

    private void ApplyTrap(HitManager manager)
    {
        var body = manager.GetComponent<Rigidbody2D>();
        StartCoroutine(Freeze(body));
    }

    private IEnumerator Freeze(Rigidbody2D body)
    {
        body.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(0.25f);

        body.constraints = RigidbodyConstraints2D.None;
    }

    public override void Initialize()
    {
        transform.rotation = transform.parent.rotation;
    }

    public override void Launch()
    {
        
    }

    public void A_Launch()
    {
        transform.rotation = transform.parent.rotation;
        direction = transform.position - transform.parent.position;
        direction.Normalize();
        StartCoroutine(LaunchSequence());
        transform.SetParent(null);
        trail.Play();
    }

    private IEnumerator LaunchSequence()
    {
        float step = 0;

        while (step <= 1)
        {
            transform.position += (1 - step) * launchSpeed * (Vector3)direction;

            step += 0.01f;
            yield return time;
        }
    }

    protected override void DisableDeployable()
    {
        if (trail != null) trail.Stop();

        base.DisableDeployable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<HitManager>(out var hit))
        {
            actionMediator.PassTarget(hit, out var damage);
        }
    }
}
