using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TitleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject highlight;
    [SerializeField] private ParticleSystem hoverParticles;
    [SerializeField] private UnityEvent eventToTrigger;
    [SerializeField] [FMODUnity.EventRef] protected string clickSFX;
    [SerializeField] [FMODUnity.EventRef] protected string hoverSFX;
    private int hoverIndex;
    private int clickIndex;
    private AudioManager audioManager;
    private bool clickable = true;

    void Start()
    {
        audioManager = AudioManager.Main;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!clickable) return;
        audioManager.RequestGUIFX(clickSFX, out clickIndex);
        eventToTrigger.Invoke();
        StartCoroutine(HandleClickActivation());
    }

    private IEnumerator HandleClickActivation()
    {
        clickable = false;
        yield return new WaitForSecondsRealtime(0.1f);
        clickable = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlight.SetActive(true);
        hoverParticles.Play();
        audioManager.RequestGUIFX(hoverSFX, out hoverIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlight.SetActive(false);
        hoverParticles.Stop();
        // audioManager.StopGUIFX(clickIndex);
        // audioManager.StopGUIFX(hoverIndex);
    }
}
