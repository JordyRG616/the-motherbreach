using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class PlusButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private StatBox statBox;
    private AudioManager audioManager;
    private Image image;
    private TextMeshProUGUI icon;

    [Header("OnHover")]
    [SerializeField] private Color hoverBoxColor;
    [SerializeField] [FMODUnity.EventRef] private string hoverSfx;
    private FMOD.Studio.EventInstance hoverSfxInstance;

    [Header("Events")]
    public UnityEvent OnClick;
    public UnityEvent OnRelease;

    [Header("OnClick")]
    [SerializeField] private Color clickIconColor;
    [SerializeField] private ParticleSystem vfx;
    [SerializeField] [FMODUnity.EventRef] private string clickSfx;
    private FMOD.Studio.EventInstance clickSfxInstance;

    [Header("OnRelease")]
    [SerializeField] private Color normalIconColor;
    [SerializeField] private Color normalBoxColor;

    private void Awake()
    {
        audioManager = AudioManager.Main;
        image = GetComponent<Image>();
        icon = GetComponentInChildren<TextMeshProUGUI>();
        Deactivate();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        icon.color = clickIconColor;
        vfx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        vfx.Play();
        audioManager.StopSFX(clickSfxInstance);
        audioManager.RequestGUIFX(clickSfx);

        OnClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = hoverBoxColor; 
        audioManager.StopSFX(hoverSfxInstance);
        audioManager.RequestGUIFX(hoverSfx);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = normalBoxColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        icon.color = normalIconColor;

        OnRelease?.Invoke();
    }

    public void Activate()
    {
        image.enabled = true;
        icon.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        image.enabled = false;
        icon.gameObject.SetActive(false);
    }
}
