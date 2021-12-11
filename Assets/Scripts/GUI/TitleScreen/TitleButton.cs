using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject highlight;
    [SerializeField] private ParticleSystem hoverParticles;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.LogWarning(name + " not yet implemented");
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
