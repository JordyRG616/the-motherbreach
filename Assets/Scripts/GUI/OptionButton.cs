using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionButton : MonoBehaviour, IPointerClickHandler
{
    private event EventHandler OnClick;
    private Image image;
    private Sprite ogSprite;
    [SerializeField] private Sprite clickedSprite;
    [SerializeField] [FMODUnity.EventRef] private string onClickSFX;

    void Awake()
    {
        image = GetComponent<Image>();
        ogSprite = image.sprite;

        OnClick += GameManager.Main.HandleOptionsMenu;
    }

    private void Update()
    {
        if(GameManager.Main.onPause) image.sprite = clickedSprite;
        else image.sprite = ogSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Main.RequestGUIFX(onClickSFX);
        OnClick?.Invoke(this, EventArgs.Empty);
    }

    public void CloseMenu()
    {
        AudioManager.Main.RequestGUIFX(onClickSFX);
        OnClick?.Invoke(this, EventArgs.Empty);
    }
}
