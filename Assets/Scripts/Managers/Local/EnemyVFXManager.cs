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

        while(step <= 1)
        {
            instMaterial.SetFloat("_death", step);
            step += .01f;
            yield return new WaitForSecondsRealtime(.01f);
        }


        //yield return new WaitForSecondsRealtime(deathParticles.main.duration);

        healthController.TriggerOnDeath();
    }

    public IEnumerator UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float percentual = Mathf.Min(1 - currentHealth / maxHealth, 1);

        instMaterial.SetFloat("_Damaged", 1);

        instMaterial.SetFloat("_damagePercentual", percentual);

        yield return new WaitForSecondsRealtime(.15f);

        instMaterial.SetFloat("_Damaged", 0);

        StopCoroutine("UpdateHealthBar");
    }
}
