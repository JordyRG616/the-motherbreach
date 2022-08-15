using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class RerrollButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public enum RerrollTarget { Weapons, Foundations }

    [SerializeField] private RerrollTarget target;
    [SerializeField] private Sprite clickSprite;
    [SerializeField] private RectTransform tipBox;
    [SerializeField] private UIAnimations cashTextAnim;
    public int rerrollCost;
    private TextMeshProUGUI tipBoxText;
    private Image image;
    private RewardManager rewardManager;
    private BuildBox buildBox;
    private Sprite ogSprite;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [SerializeField] [FMODUnity.EventRef] private string clicksSFX;
    private float counter;
    [SerializeField] private float pressTime;
    private bool pressed;
    [SerializeField] private RectMask2D mask;

    public event EventHandler OnReroll;

    void Start()
    {
        rewardManager = RewardManager.Main;
        image = GetComponent<Image>();

        ogSprite = image.sprite;

        tipBoxText = tipBox.Find("Text").GetComponent<TextMeshProUGUI>();

        buildBox = FindObjectOfType<BuildBox>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Main.RequestGUIFX(hoverSFX);
        GetComponent<ShaderAnimation>().Play();
        tipBoxText.text = "reset " + target + " (" + rerrollCost + "$)";
        tipBox.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tipBox.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        //image.sprite = clickSprite;
        pressed = true;

    }

    private void Activate()
    {
        if (rewardManager.TotalCash >= rerrollCost && !buildBox.OnUpgrade)
        {
            AudioManager.Main.RequestGUIFX(clicksSFX);

            rewardManager.SpendCash(rerrollCost);

            Reroll();
            OnReroll?.Invoke(this, EventArgs.Empty);
            return;
        }
        else
        {
            AudioManager.Main.PlayInvalidSelection("Not enough cash");
        }
    }

    public void Reroll()
    {
        if(target == RerrollTarget.Weapons)
        {
            rewardManager.EliminateWeaponOffer();
            rewardManager.GenerateWeaponOffer();
        }
        else
        {
            rewardManager.EliminateFoundationOffer();
            rewardManager.GenerateFoundationOffer();
        }

        //rewardManager.EliminateOffer();
        //rewardManager.GenerateOffer();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        counter = 0;
    }

    void Update()
    {
        if (pressed)
        {
            counter += Time.deltaTime;
            if (counter >= pressTime)
            {
                Activate();
                pressed = false;
                counter = 0;
            }
        }
        mask.padding = new Vector4(0, 0, 0, 35 * (1 - (counter / pressTime)));
    }
}
