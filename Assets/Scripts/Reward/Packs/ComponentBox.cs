using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ComponentBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image componentImage;
    [SerializeField] private bool clickable;
    [SerializeField] private GameObject selectedOverlay;
    private OfferTweaker tweaker;
    private Image image;
    private string componentName;
    private RectTransform infoBox;
    private GameObject _object;
    private bool selected;

    public delegate void ClickDelegate(GameObject component);
    public ClickDelegate OnClick;

    void Awake()
    {
        image = GetComponent<Image>();
        infoBox = FindObjectOfType<StatInfoBox>(true).GetComponent<RectTransform>();
        
        if(clickable) OnClick += HandleClick;
        tweaker = FindObjectOfType<OfferTweaker>();
    }

    private void HandleClick(GameObject component)
    {
        if(!selected)
        {
            if(tweaker.SelectionFull()) return;
            selectedOverlay.SetActive(true);
            selected = true;
        }
        else
        {    
            selectedOverlay.SetActive(false);
            selected = false;
        }
    }

    public void ConfigureBox(string name, Sprite sprite, Color color)
    {
        componentImage.sprite = sprite;
        componentName = name;
        image.color = color;
    }

    public void ReceiveComponentObject(GameObject component)
    {
        _object = component;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // GetComponent<ShaderAnimation>().Play();
        // AudioManager.Main.RequestGUIFX(hoverSFX);

        infoBox.gameObject.SetActive(true);
        infoBox.GetComponent<StatInfoBox>().SetText(componentName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBox.gameObject.SetActive(false);
    }

    void Update()
    {
        if(infoBox.gameObject.activeSelf)
        {
            Vector2 mousePos = Input.mousePosition + new Vector3(2, -2) - new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2, 0);
            infoBox.anchoredPosition = mousePos;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(_object);
    }
}
