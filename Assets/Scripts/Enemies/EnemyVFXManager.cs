using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVFXManager : VFXManager
{
    [SerializeField] private EnemyHealthController healthController;
    [SerializeField] private ParticleSystem damageSFX;
    private FMOD.Studio.EventInstance audioInstance;

    protected override void Awake()
    {
        base.Awake();
        damageSFX.Play();
    }

    public IEnumerator LastBreath()
    {
        float step = 0;

        damageSFX.Stop();
        deathParticles.Play();
        audioManager.StopSFX(audioInstance);
        audioManager.RequestSFX(deathSFX, out audioInstance);

        GetComponentInChildren<TrailRenderer>().emitting = false;
        GetComponent<EnemyAttackController>().Stop();

        GetComponent<Collider2D>().enabled = false;

        while(step <= 2)
        {
            instMaterial.SetFloat("_death", step / 2);
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.clear, step / 2);
            step += .01f;
            yield return new WaitForSeconds(.01f);
        }

    //yield return new WaitForSeconds(deathParticles.main.duration);

        healthController.TriggerOnDeath();
    }

    public IEnumerator UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float percentual = Mathf.Min(1 - currentHealth / maxHealth, 1);

        instMaterial.SetFloat("_Damaged", 1);

        SetDamageEmission(percentual);

        yield return new WaitForSeconds(.15f);

        instMaterial.SetFloat("_Damaged", 0);

        StopCoroutine("UpdateHealthBar");
    }

    private void SetDamageEmission(float percentual)
    {
        var emission = damageSFX.emission;
        emission.rateOverTimeMultiplier = percentual * 10;
    }
}
