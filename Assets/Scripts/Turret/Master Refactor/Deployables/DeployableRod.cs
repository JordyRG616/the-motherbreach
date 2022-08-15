using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployableRod : Deployable
{
    [Header("Specifics")]
    [SerializeField] private float launchSpeed;
    [SerializeField] private float rotationSpeed;
    private float rotationFactor = 0;
    [SerializeField] private ParticleSystem shooter;
    [SerializeField] private TurretActionMediator actionMediator;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    public override void Initialize()
    {
        var weapon = transform.GetComponentInParent<Weapon>();
        actionMediator.linkedWeapon = weapon;
        weapon.ReplaceShooter(shooter);
        rotationFactor = 0;

        var emission = shooter.emission;
        var value = weapon.GetStatValue<BulletsPerSecond>();
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(value);
    }

    public override void Launch()
    {
        StartCoroutine(LaunchSequence());
    }

    private IEnumerator LaunchSequence()
    {
        var direction = transform.position - transform.parent.position;
        direction.Normalize();
        float step = 0;

        transform.SetParent(null);

        while (step <= 1)
        {
            transform.position += direction * launchSpeed * (1 - step);

            step += 0.01f;
            yield return waitTime;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (rotationFactor < 1) rotationFactor += Time.deltaTime;
        var angle = transform.eulerAngles.z;
        angle += rotationSpeed * rotationFactor;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    protected override void DisableDeployable()
    {
        StopAllCoroutines();
        shooter.Stop();

        base.DisableDeployable();
    }

    public void A_Shoot()
    {
        shooter.Play();
    }
}
