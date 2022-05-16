using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BossHealthController : MonoBehaviour, IDamageable
{
    public enum CoreType {power, reinforced, noble}
    public CoreType coreType;
    [SerializeField] private int maxHealth;
    [SerializeField] [Range(0, 1)] private float damageReduction;
    [SerializeField] [FMODUnity.EventRef] private string onDamageSFX;
    [SerializeField] [FMODUnity.EventRef] private string onDeathSFX;
    [SerializeField] private ParticleSystem onDeathVFX;
    [SerializeField] private Material _material;
    private BossController bossController;
    private BossHealthBar healthBar;


    private float currentHealth;
    private RectTransform firstHealthBar;

    public event EventHandler OnDamage;
    public event EventHandler OnDeath;


    public void Initiate()
    {
        bossController = GetComponent<BossController>();
        healthBar = FindObjectOfType<BossHealthBar>();

        currentHealth = maxHealth;
        healthBar.InitiateHealthBar(maxHealth, bossController.ReturnThresholds());
        bossController.VerifyPhase();
    }


    public void DestroyDamageable()
    {
        foreach(IManager manager in GetComponents<IManager>())
        {
            manager.DestroyManager();
        }

    }

    public void TriggerOnDeath()
    {
        DestroyDamageable();
        Destroy(gameObject);
    }
    
    public void UpdateHealth(float amount)
    {
        currentHealth += amount * (1 - damageReduction);
        if(amount < 0)
        {
            healthBar.UpdateHealthBar(GetHealthPercentage());
            StartCoroutine(FlashDamage());
            AudioManager.Main.RequestSFX(onDamageSFX);
            bossController.VerifyPhase();
            OnDamage?.Invoke(this, EventArgs.Empty);
        } 
            
        if(currentHealth <= 0)
        {
            OnDeath?.Invoke(this, EventArgs.Empty);

            bossController.EndMusicInstance();
            healthBar.TerminateMarkers();
            
            AudioManager.Main.RequestSFX(onDeathSFX);

            GetComponent<Collider2D>().enabled = false;
            bossController.Sleep();
            GetComponent<Rigidbody2D>().Sleep();

            onDeathVFX.Play();

            Invoke("TriggerOnDeath", onDeathVFX.main.duration);
        }

    }

    private IEnumerator FlashDamage()
    {
        _material.SetFloat("_Damaged", 1);
        yield return new WaitForSeconds(0.1f);
        _material.SetFloat("_Damaged", 0);
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    public void UpdateHealthBar()
    {

    }

    public float GetDamageReduction()
    {
        return damageReduction;
    }

    public void SetDamageReduction(float value)
    {
        damageReduction = value;
    }

    private void RewardCore()
    {
        switch (coreType)
        {
            case CoreType.power:
                GameManager.Main.powerCoreAmount++;
            break;
            case CoreType.reinforced:
                GameManager.Main.reinforcedCoreAmount++;
            break;
            case CoreType.noble:
                GameManager.Main.nobleCoreAmount++;
            break;
        }
    }
}
