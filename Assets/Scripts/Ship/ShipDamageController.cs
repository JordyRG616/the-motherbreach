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
    [SerializeField] [ColorUsage(true, true)] private Color damageColor;
    [SerializeField] private float damageIntensity;
    private Color ogColor;
    [SerializeField] [FMODUnity.EventRef] private string onHitSFX;

    [SerializeField] private ParticleSystem deathVFX;
    [SerializeField] [FMODUnity.EventRef] private string onDeathSFX;
    [Header("UI")]
    [SerializeField] private RectTransform fill;
    [SerializeField] private TextMeshProUGUI textMesh;
    private Queue<IEnumerator> enqueuedUpdates = new Queue<IEnumerator>();
    private float ogIntensity;

    private WaitForEndOfFrame waitTime = new WaitForEndOfFrame();
    private WaitForSeconds blinkTime = new WaitForSeconds(0.15f);
    private float currentHealth
    {
        get
        {
            return _cHealth;
        }
        set
        {
            _cHealth = Mathf.FloorToInt(value);
        }
    }
    private float _cHealth;
    
    void Awake()
    {
        currentHealth = maxHealth;
        ogColor = blinkingLight.color;
        ogIntensity = blinkingLight.intensity;
        StartCoroutine(ManageGUIUpdate());
    }

    public void DestroyDamageable()
    {
        
    }

    public void UpdateHealth(float amount)
    {
        currentHealth += amount;
        if(currentHealth <= 0)
        {
            StartCoroutine(DeathAnimation());
            return;
        }

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if(amount < 0)
        {
            AudioManager.Main.RequestSFX(onHitSFX);
            StartCoroutine(Blink());
        }

        var newUpdate = UpdateGUI();
        enqueuedUpdates.Enqueue(newUpdate);
    }

    public void UpdateHealthNoEffects(float amount)
    {
        currentHealth += amount;
        if(currentHealth <= 0)
        {
            StartCoroutine(DeathAnimation());
            return;
        }
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        
        var newUpdate = UpdateGUI();
        enqueuedUpdates.Enqueue(newUpdate);
        
    }

    private IEnumerator UpdateGUI()
    {
        float step = 0;
        var percentage = currentHealth / maxHealth;
        var ogScale = fill.localScale.x;
        textMesh.text = currentHealth.ToString("0");

        while(step <= 1)
        {
            var scaleX = Mathf.Lerp(ogScale, percentage, step);
            fill.localScale = new Vector2(scaleX, fill.localScale.y);

            step += 0.1f;

            yield return waitTime;
        }

        fill.localScale = new Vector2(percentage, fill.localScale.y);

        textMesh.text = currentHealth.ToString("0");
    }

    private IEnumerator Blink()
    {
        blinkingLight.color = damageColor;
        blinkingLight.intensity = damageIntensity;

        yield return blinkTime;

        blinkingLight.color = ogColor;
        blinkingLight.intensity = ogIntensity;
    }

    private IEnumerator DeathAnimation()
    {
        GetComponent<Collider2D>().enabled = false;
        FindObjectOfType<ShipController>().DestroyController();

        AudioManager.Main.GetAudioTrack("Music").StopAllAudio();

        AudioManager.Main.RequestSFX(onDeathSFX);
        deathVFX.Play();
        float _time = Time.timeScale;

        for(int i = 0; i < 100; i++)
        {
            _time -= 0.01f;
            if(_time < 0) _time = 0;
            Time.timeScale = _time;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        yield return new WaitForEndOfFrame();

        GameManager.Main.endgamePic = ScreenCapture.CaptureScreenshotAsTexture();

        GameManager.Main.GameOver();
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

    public void ModifyHealthByPercentage(float percentage)
    {
        currentHealth += maxHealth * percentage;
        maxHealth *= (1 + percentage);
        
        var newUpdate = UpdateGUI();
        enqueuedUpdates.Enqueue(newUpdate);
    }
}
