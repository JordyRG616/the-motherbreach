using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BossHealthController : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth;
    [SerializeField] [Range(0, 1)] private float damageReduction;
    [SerializeField] [FMODUnity.EventRef] private string onDamageSFX;
    [SerializeField] [FMODUnity.EventRef] private string onDeathSFX;
    [SerializeField] private ParticleSystem onDeathVFX;
    private float currentHealth;
    private RectTransform firstHealthBar;
    private RectTransform secondHealthBar;
    private RectTransform thirdHealthBar;
    private RectTransform healthBarFill;
    private BossController bossController;

    private List<float> thresholds;

    public event EventHandler OnDamage;


    public void Initiate()
    {
        bossController = GetComponent<BossController>();

        firstHealthBar = GameObject.FindGameObjectWithTag("FirstBar").GetComponent<RectTransform>();
        secondHealthBar = GameObject.FindGameObjectWithTag("SecondBar").GetComponent<RectTransform>();
        thirdHealthBar = GameObject.FindGameObjectWithTag("ThirdBar").GetComponent<RectTransform>();
        healthBarFill = GameObject.FindGameObjectWithTag("BarFill").GetComponent<RectTransform>();

        currentHealth = maxHealth;
        InitiateHealthBar();
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
            UpdateHealthBar();
            AudioManager.Main.RequestSFX(onDamageSFX);
            bossController.VerifyPhase();
            OnDamage?.Invoke(this, EventArgs.Empty);
        } 
            
        if(currentHealth <= 0)
        {
            var animations = firstHealthBar.GetComponentsInParent<UIAnimations>();

            foreach(UIAnimations animation in animations)
            {
                animation.PlayReverse();
            }

            AudioManager.Main.RequestSFX(onDeathSFX);

            GetComponent<Collider2D>().enabled = false;
            GetComponent<BossAttackController>().StopWeapons();
            bossController.Sleep();
            GetComponent<Rigidbody2D>().Sleep();

            onDeathVFX.Play();

            Invoke("TriggerOnDeath", onDeathVFX.main.duration);
        }

    }

    private void InitiateHealthBar()
    {
        thresholds = bossController.ReturnThresholds();
        var height = firstHealthBar.sizeDelta.y;

        firstHealthBar.sizeDelta = new Vector2(maxHealth * (thresholds[0] - thresholds[1]), height);
        secondHealthBar.sizeDelta = new Vector2(maxHealth * (thresholds[1] - thresholds[2]), height);
        thirdHealthBar.sizeDelta = new Vector2(maxHealth * thresholds[2], height);
        healthBarFill.sizeDelta = new Vector2(maxHealth - 35, healthBarFill.sizeDelta.y);

        var animations = firstHealthBar.GetComponentsInParent<UIAnimations>();

        foreach(UIAnimations animation in animations)
        {
            animation.Play();
        }
    }

    public void UpdateHealthBar()
    {
        healthBarFill.sizeDelta = new Vector2
            (
                GetHealthPercentage() * (maxHealth - 35),
                healthBarFill.sizeDelta.y
            );
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}
