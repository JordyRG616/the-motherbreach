using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LevelUpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite clickedSprite;
    [SerializeField] private RectTransform expBar;
    [SerializeField] private RectTransform expAmount;
    [SerializeField] private TextMeshProUGUI lvlText;
    [SerializeField] private RectTransform tipBox;
    private TextMeshProUGUI tipText;
    private Sprite ogSprite;
    private Image image;
    private RewardCalculator rewardCalculator;

    void Start()
    {
        image = GetComponent<Image>();
        ogSprite = image.sprite;
        rewardCalculator = FindObjectOfType<RewardCalculator>();
        UpdateExpGui();

        tipText = tipBox.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = clickedSprite;
        rewardCalculator.PurchaseLevelUp();
        UpdateExpGui();
    }

    private void UpdateExpGui()
    {
        lvlText.text = "lvl" + rewardCalculator.ShopLevel;
        var requiredHeight = (rewardCalculator.ExpInfo().required * 12) + 4;
        expBar.sizeDelta = new Vector2(expBar.sizeDelta.x, requiredHeight);
        var amountHeight = (rewardCalculator.ExpInfo().amount * 12) + 4;
        expAmount.sizeDelta = new Vector2(expAmount.sizeDelta.x, amountHeight);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = ogSprite;
    }

    private void Update()
    {
        if(tipBox.gameObject.activeSelf)
        {
            Vector2 mousePos = Input.mousePosition + new Vector3(2, -2) - new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2, 0);
            tipBox.anchoredPosition = mousePos;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tipText.text = "level up (" + (rewardCalculator.ShopLevel + 1) + "$)" ;
        tipBox.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tipBox.gameObject.SetActive(false);
    }
}
