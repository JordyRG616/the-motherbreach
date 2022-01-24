using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class RerrollButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Sprite clickSprite;
    [SerializeField] private RectTransform tipBox;
    [SerializeField] private UIAnimations cashTextAnim;
    private TextMeshProUGUI tipBoxText;
    private Image image;
    private RewardManager rewardManager;
    private Sprite ogSprite;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [SerializeField] [FMODUnity.EventRef] private string clicksSFX;
    private int offerTimelineIndex;

    void Start()
    {
        rewardManager = RewardManager.Main;
        image = GetComponent<Image>();

        ogSprite = image.sprite;

        tipBoxText = tipBox.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Main.RequestGUIFX(hoverSFX);
        GetComponent<ShaderAnimation>().Play();
        tipBoxText.text = "reset (2$)";
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

    public void OnPointerDown(PointerEventData eventData)
    {
        var locked = FindObjectOfType<LockButton>().locked;

        if(rewardManager.TotalCash >= 2 && !locked)
        {
            AudioManager.Main.RequestGUIFX(clicksSFX);
            image.sprite = clickSprite;

            rewardManager.SpendedCash = 2;
            cashTextAnim.PlayReverse();

            rewardManager.EliminateOffer();
            rewardManager.GenerateOffer();
            return;
        }
        AudioManager.Main.PlayInvalidSelection();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Invoke("ResetSprite", .1f);
    }

    private void ResetSprite()
    {
        image.sprite = ogSprite;
    }
}
