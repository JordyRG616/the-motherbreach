using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SellButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    private TextMeshProUGUI textMesh;
    private int refund;
    private UIAnimations cashTextAnimation;
    private RewardManager rewardManager;
    private BuildBox buildBox;
    private TurretSlot cachedSlot;
    private InputManager inputManager;
    private bool onCoroutine;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [SerializeField] [FMODUnity.EventRef] private string sellSFX;
    [SerializeField] [FMODUnity.EventRef] private string replaceSFX;
    private float counter;
    [SerializeField] private float pressTime;
    private bool pressed;
    [SerializeField] private RectMask2D mask;

    public event EventHandler OnTurretSell;

    void Awake()
    {
        textMesh = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        cashTextAnimation = FindObjectOfType<CashTextAnimation>();
        rewardManager = RewardManager.Main;
        inputManager = InputManager.Main;
        buildBox = FindObjectOfType<BuildBox>();
    }

    void OnEnable()
    {
        inputManager.OnSelectionClear += Disable;
    }

    public void Disable(object sender, EventArgs e)
    {
        Disable();
    }
    public void Disable()
    {
        if(cachedSlot ==  null) return;
        if(!gameObject.activeSelf) return;
        inputManager.OnSelectionClear -= Disable;
        gameObject.SetActive(false);
        cachedSlot = null;
    }
    public void SetButton(int refund, TurretSlot turretSlot)
    {
        textMesh.text = "sell" + " (" + refund +"$)";

        this.refund = refund;
        cachedSlot = turretSlot;
    }

    private void Update()
    {
        if (pressed)
        {
            counter += Time.deltaTime;

            if (counter >= pressTime)
            {
                StartCoroutine(Sell());
                OnTurretSell?.Invoke(this, EventArgs.Empty);
                pressed = false;
                counter = 0;
            }
        }

        mask.padding = new Vector4(0, 0, 0, 44 * (1 - (counter / pressTime)));
    }

    public void Replace()
    {
        var _base = buildBox.baseToReplace;
        if (_base == null) 
        {
            AudioManager.Main.PlayInvalidSelection("Select a base to replace");
            return;
        }
        var cost = _base.GetComponent<BaseEffectTemplate>().GetCost();
        if (rewardManager.TotalCash >= cost)
        {
            AudioManager.Main.RequestGUIFX(replaceSFX);
            rewardManager.SpendCash((int)cost);
            buildBox.selectedBaseBox.Detach();
            buildBox.UpdateStats();
            buildBox.baseToReplace = null;
        }
        else AudioManager.Main.PlayInvalidSelection("Not enough cash");
    }

    private IEnumerator Sell()
    {
        var vfx = GetComponentInChildren<ParticleSystem>();
        vfx.Play();
        AudioManager.Main.RequestGUIFX(sellSFX);


        rewardManager.EarnedCash = refund;
        cashTextAnimation.Play();

        GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);
        GetComponent<Image>().enabled = false;

        cachedSlot.GetComponentInChildren<ParticleSystem>().Stop();
        cachedSlot.occupyingTurret.GetComponent<IntegrityManager>().SellTurret();
        cachedSlot.Clear();

        yield return new WaitUntil(() => !vfx.IsAlive());

        GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
        GetComponent<Image>().enabled = true;

        gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<ShaderAnimation>().Play();
        AudioManager.Main.RequestGUIFX(hoverSFX);
    }

    void OnDisable()
    {
        inputManager.OnSelectionClear -= Disable;
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
