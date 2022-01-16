using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite clickSprite;
    [SerializeField] private RectTransform tipBox;
    private TextMeshProUGUI tipBoxText;
    private Image image;
    private Sprite ogSprite;
    private UIAnimationManager animationManager;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [SerializeField] [FMODUnity.EventRef] private string clicksSFX;

    void Start()
    {
        animationManager = FindObjectOfType<UIAnimationManager>();
        image = GetComponent<Image>();
        ogSprite = image.sprite;
        tipBoxText = tipBox.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Main.RequestGUIFX(clicksSFX);
        image.sprite = clickSprite;
        animationManager.Reverse();
        Invoke("ResetSprite", 5f);
    }

    private void ResetSprite()
    {
        image.sprite = ogSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Main.RequestGUIFX(hoverSFX);
        GetComponent<ShaderAnimation>().Play();
        tipBoxText.text = "next wave";
        tipBox.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tipBox.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(tipBox.gameObject.activeSelf)
        {
            Vector2 mousePos = Input.mousePosition + new Vector3(2, -2) - new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2, 0);
            tipBox.anchoredPosition = mousePos;
        }
    }
}
