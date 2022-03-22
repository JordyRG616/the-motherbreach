using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
    [SerializeField] [FMODUnity.EventRef] private string onHitSFX;
    [SerializeField] [FMODUnity.EventRef] private string onSpawnSFX;
    [SerializeField] [FMODUnity.EventRef] private string destructionSFX;
    private Material _material;
    private Color ogColor;
    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.OnGameStateChange += DestroyShield;

        currentHealth = maxHealth;
        _material = new Material(GetComponent<SpriteRenderer>().material);
        GetComponent<SpriteRenderer>().material = _material;
        ogColor = _material.GetColor("_Shield_Color");

        AudioManager.Main.RequestSFX(onSpawnSFX);
    }

    private void DestroyShield(object sender, GameStateEventArgs e)
    {
        if(e.newState == GameState.OnReward)
        {
            gameManager.OnGameStateChange -= DestroyShield;
            Destroy(gameObject);
        }
    }

    public void DestroyDamageable()
    {
        StartCoroutine(DestructionAnimation());
    }

    private IEnumerator DestructionAnimation()
    {
        var scale = Vector3.zero;
        float step = 0;

        AudioManager.Main.RequestSFX(destructionSFX);

        while(step < 1)
        {
            scale += Vector3.one / 20;
            transform.localScale += scale;
            _material.SetFloat("_Intensity", step + 1);
            step += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }

        gameManager.OnGameStateChange -= DestroyShield;
        Destroy(gameObject);
    }

    public void UpdateHealth(float amount)
    {
        currentHealth += amount;


        if(amount < 0)
        {
            UpdateHealthBar();
            AudioManager.Main.RequestSFX(onHitSFX);
            _material.SetFloat("_Intensity", 0);
            Invoke("ResetIntensity", .1f);

        }
        if(currentHealth <= 0) DestroyDamageable();
    }

    private void ResetIntensity()
    {
        _material.SetFloat("_Intensity", 1);
    }

    public void UpdateHealthBar()
    {
        // var color = Color.Lerp(fullDamageColor, ogColor, currentHealth / maxHealth);
        _material.SetFloat("_Damage", 1 - (currentHealth / maxHealth));
    }
    
    public void RaiseHealthByPercentage(float percentage)
    {
        maxHealth *= (1 + percentage);
        currentHealth *= (1 + percentage);
        UpdateHealthBar();
    }
}
