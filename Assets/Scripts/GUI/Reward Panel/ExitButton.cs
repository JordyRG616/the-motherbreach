using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

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
    [Header("End of wave animations")]
    [SerializeField] private UIAnimations upperTextAnimation;
    [SerializeField] private UIAnimations lowerTextAnimation;
    [SerializeField] private UpgradeButton upgradeButton;
    [SerializeField] private SellButton sellButton;

    void Start()
    {
        animationManager = GameObject.FindGameObjectWithTag("RewardAnimation").GetComponent<UIAnimationManager>();

        image = GetComponent<Image>();
        ogSprite = image.sprite;
        tipBoxText = tipBox.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(ExitReward());
    }

    private IEnumerator ExitReward()
    {
        AudioManager.Main.RequestGUIFX(clicksSFX);
        image.sprite = clickSprite;
        Invoke("ResetSprite", .1f);

        DeactivateButtons();

        yield return StartCoroutine(animationManager.ReverseTimeline());

        yield return StartCoroutine(PlayEndWaveAnimation());

        RewardManager.Main.Exit();
        // Camera.main.GetComponent<Animator>().enabled = false;
    }

    private void DeactivateButtons()
    {
        upgradeButton.Disable();
        sellButton.Disable();
    }

    private IEnumerator PlayEndWaveAnimation()
    {
        yield return StartCoroutine(upperTextAnimation.Forward());

        upperTextAnimation.PlayReverse();
        yield return StartCoroutine(lowerTextAnimation.Forward());

        lowerTextAnimation.PlayReverse();
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
}
