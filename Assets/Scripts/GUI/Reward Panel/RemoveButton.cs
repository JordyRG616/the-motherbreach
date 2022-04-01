using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RemoveButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Sprite clickSprite;
    private Sprite ogSprite;
    private Image image;

    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    private OfferTweaker tweaker;


    void Start()
    {
        tweaker = FindObjectOfType<OfferTweaker>();
        image = GetComponent<Image>();
        ogSprite = image.sprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = clickSprite;
        tweaker.Remove();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {   
        AudioManager.Main.RequestGUIFX(hoverSFX);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = ogSprite;
    }
}
