using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class LevelUpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite clickedSprite;
    [SerializeField] private RectTransform expBar;
    [SerializeField] private RectTransform expAmount;
    [SerializeField] private TextMeshProUGUI lvlText;
    [SerializeField] private RectTransform tipBox;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [Header("VFX")]
    [SerializeField] private ParticleSystem expBarVFX;
    private float requiredHeight;
    private float amountHeight;
    private bool active = true;
    
    private TextMeshProUGUI tipText;
    private Sprite ogSprite;
    private Image image;
    private RewardCalculator rewardCalculator;

    void Start()
    {
        image = GetComponent<Image>();
        ogSprite = image.sprite;
        rewardCalculator = FindObjectOfType<RewardCalculator>();
        expAmount.sizeDelta = new Vector2(expAmount.sizeDelta.x, 0);
        expBar.sizeDelta = new Vector2(expBar.sizeDelta.x, 0);
        UpdateExpGui();


        tipText = tipBox.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!active)
        {
            AudioManager.Main.PlayInvalidSelection();
            return;
        }
        image.sprite = clickedSprite;
        rewardCalculator.PurchaseLevelUp();
        UpdateExpGui();
        if(rewardCalculator.ShopLevel == rewardCalculator.maxShopLevel) DisableButton();
    }

    private void DisableButton()
    {
        lvlText.text = "max tier";
        image.sprite = clickedSprite;
        expBar.gameObject.SetActive(false);
        expAmount.gameObject.SetActive(false);
        active = false;
    }

    public void GainExp()
    {
        rewardCalculator.GainExp();
        UpdateExpGui();
    }

    private void UpdateExpGui()
    {
        lvlText.text = "tier <size=110%>" + rewardCalculator.ShopLevel;
        requiredHeight = (rewardCalculator.ExpInfo().required * 12) + 4;
        // expBar.sizeDelta = new Vector2(expBar.sizeDelta.x, requiredHeight);
        amountHeight = (rewardCalculator.ExpInfo().amount * 12) + 4;
        // expAmount.sizeDelta = new Vector2(expAmount.sizeDelta.x, amountHeight);
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = ogSprite;
    }

    private void Update()
    {
        if (tipBox.gameObject.activeSelf)
        {
            Vector2 mousePos = Input.mousePosition + new Vector3(2, -2) - new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0);
            tipBox.anchoredPosition = mousePos;
        }

        HandleExpBar();

        if(Mathf.Approximately(expBar.sizeDelta.y, expAmount.sizeDelta.y) && expAmount.sizeDelta.y > 0)
        {
            rewardCalculator.InitiateChoice();
            UpdateExpGui();
        }
    }

    private void HandleExpBar()
    {
        if (expBar.sizeDelta.y < requiredHeight)
        {
            expBar.sizeDelta += new Vector2(0, Time.deltaTime * 100);
        }
        if (expAmount.sizeDelta.y < amountHeight)
        {
            expBarVFX.Play();
            expAmount.sizeDelta += new Vector2(0, Time.deltaTime * 100);
        }
        if (expBar.sizeDelta.y >= requiredHeight)
        {
            expBar.sizeDelta = new Vector2(expBar.sizeDelta.x, requiredHeight);
        }
        if (expAmount.sizeDelta.y >= amountHeight)
        {
            expBarVFX.Stop();
            expAmount.sizeDelta = new Vector2(expAmount.sizeDelta.x, amountHeight);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<ShaderAnimation>().Play();
        AudioManager.Main.RequestGUIFX(hoverSFX);
        tipText.text = "level up (" + (rewardCalculator.ShopLevel + 1) + "$)" ;
        tipBox.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tipBox.gameObject.SetActive(false);
    }
}
