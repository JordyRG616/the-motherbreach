using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVFXManager : VFXManager
{
    [SerializeField] private EnemyHealthController healthController;

    protected override void Awake()
    {
        base.Awake();
    }

    public IEnumerator LastBreath()
    {
        float step = 0;

        deathParticles.Play();

        GetComponentInChildren<TrailRenderer>().emitting = false;

        GetComponent<Collider2D>().enabled = false;

        while(step <= 2)
        {
            instMaterial.SetFloat("_death", step / 2);
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.clear, step / 2);
            step += .01f;
            yield return new WaitForSeconds(.01f);
        }

    //yield return new WaitForSeconds(deathParticles.main.duration);
        audioManager.RequestSFX(deathSFX);

        healthController.TriggerOnDeath();
    }

    public IEnumerator UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float percentual = Mathf.Min(1 - currentHealth / maxHealth, 1);

        instMaterial.SetFloat("_Damaged", 1);

        instMaterial.SetFloat("_damagePercentual", percentual);

        yield return new WaitForSeconds(.15f);

        instMaterial.SetFloat("_Damaged", 0);

        StopCoroutine("UpdateHealthBar");
    }
}
