using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class OfferBox : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image TopImage;
    [SerializeField] private Image baseImage;
    [SerializeField] public bool Empty {get; private set;} = true;

    public event EventHandler OnOfferSelected;

    public void ReceiveTurret(GameObject turret)
    {
        SpriteRenderer[] spriteRenderers = turret.GetComponentsInChildren<SpriteRenderer>(true);
        
        TopImage.sprite = spriteRenderers[2].sprite;
        TopImage.color = spriteRenderers[2].color;
        baseImage.sprite = spriteRenderers[1].sprite;
        baseImage.color = spriteRenderers[1].color;

        Empty = false;
    }

    public void Clear()
    {
        Empty = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnOfferSelected?.Invoke(this, EventArgs.Empty);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
