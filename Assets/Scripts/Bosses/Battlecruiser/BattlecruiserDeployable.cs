using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlecruiserDeployable : MonoBehaviour, IDamageable
{
    public Transform cannon;
    [SerializeField] private ActionEffect retaliationWeapon;
    [SerializeField] private float speed;
    [SerializeField] private int maxHealth;

    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] [FMODUnity.EventRef] private string hitSFX;
    [SerializeField] [FMODUnity.EventRef] private string deathSFX;

    private Material _material;
    private float currentHealth;
    private float counter;
    private bool fixedPosition;
    private float turnSpeed;
    private float sing;

    private Transform ship;
    private BossHealthController bossHealth;

    public void Initiate(Transform parent)
    {
        currentHealth = maxHealth;
        ship = ShipManager.Main.transform;

        _material = new Material(GetComponent<SpriteRenderer>().material);
        GetComponent<SpriteRenderer>().material = _material;
        cannon.GetComponent<SpriteRenderer>().material = _material;

        retaliationWeapon.Initiate();
        retaliationWeapon.ReceiveTarget(ShipManager.Main.gameObject);
        sing = Mathf.Sign(Random.Range(-1, 1f));

        bossHealth = parent.GetComponent<BossHealthController>();
        bossHealth.OnDeath += SuddenDeath;
    }

    private void SuddenDeath(object sender, System.EventArgs e)
    {
        UpdateHealth(-currentHealth);
    }

    public void LateInitiate()
    {
        fixedPosition = true;
        GetComponent<Collider2D>().enabled = true;
        retaliationWeapon.Shoot();
    }

    

    public void DestroyDamageable()
    {
        Destroy(gameObject);
    }

    public void UpdateHealth(float amount)
    {
        currentHealth += amount;
        AudioManager.Main.RequestSFX(hitSFX);
        if(currentHealth <= 0)
        {
            StartCoroutine(LastBreath());
        }
        else UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        StartCoroutine(DamageEffect());
    }

    private IEnumerator DamageEffect()
    {
        _material.SetFloat("_Damaged", 1);

        _material.SetFloat("_damagePercentual", currentHealth / maxHealth);

        yield return new WaitForSeconds(.15f);

        _material.SetFloat("_Damaged", 0);
    }

    public IEnumerator LastBreath()
    {
        bossHealth.OnDeath -= SuddenDeath;

        float step = 0;

        deathParticles.Play();
        retaliationWeapon.StopShooting();
        AudioManager.Main.RequestSFX(deathSFX);

        GetComponent<Collider2D>().enabled = false;

        while(step <= 1)
        {
            _material.SetFloat("_death", step / 1);
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.clear, step / 1);
            step += .01f;
            yield return new WaitForSeconds(.01f);
        }

        DestroyDamageable();
    }

    void FixedUpdate()
    {
        if(!fixedPosition) return;
        var direction = transform.position - ship.position;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var _angle = Mathf.SmoothDampAngle(cannon.eulerAngles.z, angle, ref turnSpeed, .1f, .1f);
        cannon.rotation = Quaternion.Euler(0, 0, angle + 90);
    }

    

    private IEnumerator Orbit()
    {
        while(true)
        {
            var direction = transform.position - ship.position;

            var angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + 90);

            direction += (Vector3)Vector2.Perpendicular(direction) * speed;

            transform.position = ship.position + (direction.normalized * 7);

            yield return new WaitForSeconds(0.01f);
        }

    }
}
