using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVFXManager : VFXManager
{
    [SerializeField] private EnemyHealthController healthController;
    private FMOD.Studio.EventInstance audioInstance;

    protected override void Awake()
    {
        base.Awake();
    }

    public IEnumerator LastBreath()
    {
        float step = 0;

        deathParticles.Play();
        audioManager.StopSFX(audioInstance);
        audioManager.RequestSFX(deathSFX, out audioInstance);

        GetComponent<EnemyAttackController>().Stop();
        GetComponent<EnemyAttackController>().Sleeping = true;

        GetComponent<Collider2D>().enabled = false;

        while(step <= 2)
        {
            instMaterial.SetFloat("_death", step / 2);
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.clear, step / 2);
            step += .01f;
            yield return new WaitForSeconds(.01f);
        }

        // yield return new WaitUntil(() => !deathParticles.isPlaying);

        healthController.TriggerOnDeath();
    }

    public IEnumerator UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float percentual = Mathf.Min(1 - currentHealth / maxHealth, 1);

        instMaterial.SetFloat("_Damaged", 1);

        instMaterial.SetFloat("_damagePercentual", percentual);

        // SetDamageEmission(percentual);

        yield return new WaitForSeconds(.15f);

        instMaterial.SetFloat("_Damaged", 0);

        StopCoroutine("UpdateHealthBar");
    }
}
