using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    [SerializeField] [ColorUsage(true, true)] private Color fullDamageColor;
    private float currentHealth;
    [SerializeField] [FMODUnity.EventRef] private string onHitSFX;
    private Material _material;
    private Color ogColor;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.OnGameStateChange += DestroyShield;

        currentHealth = maxHealth;
        _material = new Material(GetComponent<SpriteRenderer>().material);
        GetComponent<SpriteRenderer>().material = _material;
    }

    private void DestroyShield(object sender, GameStateEventArgs e)
    {
        if(e.newState == GameState.OnReward)
        {
            Destroy(gameObject);
        }
    }

    public void DestroyDamageable()
    {
        Destroy(gameObject);
    }

    public void UpdateHealth(float amount)
    {
        currentHealth += amount;


        if(amount < 0)
        {
            var color = Color.Lerp(ogColor, fullDamageColor, currentHealth / maxHealth);
            _material.SetColor("_Shield_Color", color);
            AudioManager.Main.RequestSFX(onHitSFX);
        }
        if(currentHealth <= 0) DestroyDamageable();
    }

    public void UpdateHealthBar()
    {

    }
    
}
