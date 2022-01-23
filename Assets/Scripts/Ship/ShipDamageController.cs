using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShipDamageController : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    [SerializeField] private Light2D blinkingLight;
    [SerializeField] private Color damageColor;
    private Color ogColor;
    [SerializeField] [FMODUnity.EventRef] private string onHitSFX;
    [Header("UI")]
    [SerializeField] private RectTransform fill;
    [SerializeField] private TextMeshProUGUI textMesh;
    private Queue<IEnumerator> enqueuedUpdates = new Queue<IEnumerator>();

    private WaitForSeconds waitTime = new WaitForSeconds(0.1f);

    private float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
        ogColor = blinkingLight.color;
        StartCoroutine(ManageGUIUpdate());
    }

    public void DestroyDamageable()
    {
        
    }

    public void UpdateHealth(float amount)
    {
        currentHealth += amount;
        if(currentHealth == 0)
        {
            GameManager.Main.GameOver();
            return;
        }
        if(amount < 0)
        {
            AudioManager.Main.RequestSFX(onHitSFX);
            StartCoroutine(Blink());
            var newUpdate = UpdateGUI();
            enqueuedUpdates.Enqueue(newUpdate);
        }
    }

    private IEnumerator UpdateGUI()
    {
        float step = 0;
        var percentage = currentHealth / maxHealth;
        var ogScale = fill.localScale.x;

        while(step <= 1)
        {
            var scaleX = Mathf.Lerp(ogScale, percentage, step);
            fill.localScale = new Vector2(scaleX, fill.localScale.y);

            textMesh.text = (scaleX * maxHealth).ToString("0");

            step += 0.1f;

            yield return waitTime;
        }

        textMesh.text = currentHealth.ToString("0");
    }

    private IEnumerator Blink()
    {
        blinkingLight.color = damageColor;

        yield return waitTime;

        blinkingLight.color = ogColor;
    }

    public void UpdateHealthBar()
    {
        
    }

    private IEnumerator ManageGUIUpdate()
    {
        while(true)
        {
            if(enqueuedUpdates.Count > 0)
            {
                yield return StartCoroutine(enqueuedUpdates.Dequeue());
            }

            yield return waitTime;
        }
    }

}
