using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ExitButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler
{
    [SerializeField] private GameObject blinkingText;
    [SerializeField] private GameObject buttonText;
    [SerializeField] private Image button;
    [SerializeField] private RectTransform tipBox;
    private TextMeshProUGUI tipBoxText;
    private Image image;
    private UIAnimationManager animationManager;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [SerializeField] [FMODUnity.EventRef] private string clicksSFX;
    [Header("End of wave animations")]
    [SerializeField] private UIAnimations upperTextAnimation;
    [SerializeField] private UIAnimations lowerTextAnimation;
    [SerializeField] private UpgradeButton upgradeButton;
    [SerializeField] private SellButton sellButton;
    private float counter;
    [SerializeField] private float pressTime;
    private bool pressed;
    [SerializeField] private RectMask2D mask;
    private BackgroundManager backgroundManager;

    void Start()
    {
        animationManager = GameObject.FindGameObjectWithTag("RewardAnimation").GetComponent<UIAnimationManager>();
        backgroundManager = FindObjectOfType<BackgroundManager>();

        image = GetComponent<Image>();
        tipBoxText = tipBox.Find("Text").GetComponent<TextMeshProUGUI>();

        StartCoroutine(WatchTurretCount());
    }

    private IEnumerator ExitReward()
    {
        backgroundManager.ChangeBackground();
        AudioManager.Main.RequestGUIFX(clicksSFX);

        DeactivateButtons();

        yield return StartCoroutine(animationManager.ReverseTimeline());

        yield return StartCoroutine(PlayEndWaveAnimation());

        RewardManager.Main.Exit();
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

    private IEnumerator WatchTurretCount()
    {
        var shipManager = ShipManager.Main;

        while(true)
        {
            button.enabled = false;
            buttonText.SetActive(false);
            blinkingText.SetActive(true);

            yield return new WaitUntil(() => shipManager.GetTurretCount() > 0);

            button.enabled = true;
            buttonText.SetActive(true);
            blinkingText.SetActive(false);

            yield return new WaitUntil(() => shipManager.GetTurretCount() == 0);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Main.RequestGUIFX(hoverSFX);
        GetComponent<ShaderAnimation>().Play();
    }

    private void Update()
    {
        if (pressed)
        {
            counter += Time.deltaTime;

            if (counter >= pressTime)
            {
                StartCoroutine(ExitReward());
                pressed = false;
                counter = 0;
            }
        }

        mask.padding = new Vector4(0, 0, 218 * (1 - (counter / pressTime)));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        counter = 0;
    }
}
