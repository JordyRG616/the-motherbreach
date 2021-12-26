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

    public void OnPointerClick(PointerEventData eventData)
    {
        eventToTrigger.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlight.SetActive(true);
        hoverParticles.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlight.SetActive(false);
        hoverParticles.Stop();
    }
}
