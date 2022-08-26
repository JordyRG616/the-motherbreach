using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVFXManager : VFXManager
{
    [SerializeField] private EnemyHealthController healthController;
    [SerializeField] private float deathDuration = 2;
    private FMOD.Studio.EventInstance audioInstance;


    protected override void Awake()
    {
        base.Awake();
    }

    public void LastBreath()
    {
        deathParticles.transform.SetParent(null);
        deathParticles.Play();
        audioManager.StopSFX(audioInstance);
        audioManager.RequestSFX(deathSFX, out audioInstance);

        if(TryGetComponent<EnemyAttackController>(out var controller))
        {
            controller.Stop();
            controller.Sleeping = true;
        }

        GetComponent<Collider2D>().enabled = false;

        //while(step <= deathDuration)
        //{
        //    instMaterial.SetFloat("_death", step / deathDuration);
        //    GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.clear, step / deathDuration);
        //    step += .01f;
        //    yield return new WaitForSeconds(.01f);
        //}

        //// yield return new WaitUntil(() => !deathParticles.isPlaying);

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
